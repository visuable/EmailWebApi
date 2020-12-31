using System;
using System.Threading.Tasks;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Entities.Dto;
using EmailWebApi.Db.Entities.Settings;
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

        private ThrottlingStateDto _latestStateDto;

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
            _latestStateDto = await _databaseService.GetThrottlingStateAsync();
            if (ConsumeTime() && ConsumeCounter())
            {
                var result = await _emailService.Send(email);
                _latestStateDto.UpdateAfterSending(email.Content.Address);
                return result;
            }

            if (ConsumeTime() && !ConsumeCounter())
            {
                email.SetEmailInfo();
                email.SetState(EmailStatus.Query);
                await _databaseService.AddEmailAsync(email);
                return email.Info;
            }

            return new EmailInfo
            {
                Date = DateTime.Now,
                UniversalId = Guid.Empty
            };
        }

        private bool ConsumeTime()
        {
            var result = false;
            try
            {
                result = DateTimeOffset.Now.ToUnixTimeSeconds() <=
                         new DateTimeOffset(_latestStateDto.EndPoint).ToUnixTimeSeconds();
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
                result = _latestStateDto.Counter < _options.Value.Limit &&
                         _latestStateDto.LastAddressCounter < _options.Value.AddressLimit;
            }
            catch
            {
                _logger.LogError("Ошибка проверки счетчика запросов");
            }

            return result;
        }
    }
}