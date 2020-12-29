using System;
using System.Threading.Tasks;
using EmailWebApi.Entities;
using EmailWebApi.Entities.Settings;
using EmailWebApi.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EmailWebApi.Services
{
    public class ThrottlingService : IThrottlingService
    {
        private readonly IDatabaseManagerService _databaseService;
        private readonly IEmailTransferService _emailService;
        private readonly ILogger<ThrottlingService> _logger;
        private readonly IOptions<ThrottlingSettings> _options;

        private ThrottlingState _latestState;

        public ThrottlingService(ILogger<ThrottlingService> logger, IEmailTransferService emailService,
            IOptions<ThrottlingSettings> options, IDatabaseManagerService databaseService)
        {
            _logger = logger;
            _emailService = emailService;
            _options = options;
            _databaseService = databaseService;
        }

        public async Task<EmailInfo> Invoke(Email email)
        {
            _latestState = await _databaseService.GetLastThrottlingStateAsync();
            if (ConsumeTime() && ConsumeCounter())
            {
                var result = await _emailService.Send(email);
                _latestState.UpdateAfterSending(email.Content.Address);
                SaveThrottlingState();
                return result;
            }

            if (ConsumeTime() && !ConsumeCounter())
            {
                email.SetEmailInfo();
                email.SetState(EmailStatus.Query);
                await _databaseService.AddEmailAsync(email);
                return email.Info;
            }

            if (!ConsumeTime())
            {
                _latestState.Refresh();
                SaveThrottlingState();
                return await Invoke(email);
            }

            return new EmailInfo
            {
                Date = DateTime.Now,
                UniversalId = Guid.Empty
            };
        }

        private async Task SaveThrottlingState()
        {
            await _databaseService.AddThrottlingStateAsync(new ThrottlingState
            {
                Counter = _latestState.Counter,
                EndPoint = _latestState.EndPoint,
                LastAddress = _latestState.LastAddress,
                LastAddressCounter = _latestState.LastAddressCounter
            });
        }

        private bool ConsumeTime()
        {
            var result = false;
            try
            {
                result = DateTimeOffset.Now.ToUnixTimeSeconds() <=
                         new DateTimeOffset(_latestState.EndPoint).ToUnixTimeSeconds();
                _logger.LogDebug($"Результат проверки по времени: {result}");
            }
            catch
            {
                _logger.LogError("Ошибка проверки времени");
            }

            return result;
        }

        private bool ConsumeCounter()
        {
            var result = false;
            try
            {
                result = _latestState.Counter < _options.Value.Limit &&
                         _latestState.LastAddressCounter < _options.Value.AddressLimit;
            }
            catch
            {
                _logger.LogError("Ошибка проверки счетчика запросов");
            }

            return result;
        }
    }
}