using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWebApi.Extensions;
using EmailWebApi.Objects;
using EmailWebApi.Objects.Settings;
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

        private ThrottlingState latestState;

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
            latestState = await _databaseService.GetLastThrottlingState();
            if (ConsumeTime() && ConsumeCounter())
            {
                var result = await _emailService.Send(email);
                latestState.UpdateAfterSending(email.Content.Address);
                SaveThrottlingState();
                return result;
            }
            else if (ConsumeTime() && !ConsumeCounter())
            {
                email.SetEmailInfo();
                email.SetState(EmailStatus.Query);
                await _databaseService.AddEmail(email);
                return email.Info;
            }
            else if (!ConsumeTime())
            {
                latestState.Refresh();
                SaveThrottlingState();
                //Оправданно тем, что это легче, чем создать state-machine.
                return await Invoke(email);
            }
            else
            {
                return new EmailInfo()
                {
                    Date = DateTime.Now,
                    UniversalId = Guid.Empty
                };
            }
        }

        private async Task SaveThrottlingState()
        {
            await _databaseService.AddThrottlingState(new ThrottlingState()
            {
                Counter = latestState.Counter,
                EndPoint = latestState.EndPoint,
                LastAddress = latestState.LastAddress,
                LastAddressCounter = latestState.LastAddressCounter
            });
        }
        private bool ConsumeTime()
        {
            var result = false;
            try
            {
                result = DateTimeOffset.Now.ToUnixTimeSeconds() <= new DateTimeOffset(latestState.EndPoint).ToUnixTimeSeconds();
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
                result = latestState.Counter < _options.Value.Limit && latestState.LastAddressCounter < _options.Value.AddressLimit;
            }
            catch
            {
                _logger.LogError("Ошибка проверки счетчика запросов");
            }
            return result;
        }

    }
}