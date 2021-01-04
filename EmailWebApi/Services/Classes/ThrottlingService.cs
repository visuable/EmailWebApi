using System;
using System.Threading.Tasks;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Entities.Dto;
using EmailWebApi.Db.Entities.Settings;
using EmailWebApi.Db.Repositories;
using EmailWebApi.Extensions;
using EmailWebApi.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EmailWebApi.Services.Classes
{
    /// <summary>
    /// Ограничивающий сервис по количеству запросов в минуту.
    /// </summary>
    public class ThrottlingService : IThrottlingService
    {
        private readonly IDateTimeService _dateTime;
        private readonly IRepository<Email> _emailRepository;
        private readonly IEmailTransferService _emailService;
        private readonly ILogger<ThrottlingService> _logger;
        private readonly IOptions<ThrottlingSettings> _options;
        private readonly IThrottlingStateProviderService _stateProvider;

        private ThrottlingStateDto _latestState;

        public ThrottlingService(ILogger<ThrottlingService> logger, IEmailTransferService emailService,
            IOptions<ThrottlingSettings> options, IRepository<Email> emailRepository,
            IThrottlingStateProviderService stateProvider, IDateTimeService dateTime)
        {
            _logger = logger;
            _emailService = emailService;
            _options = options;
            _emailRepository = emailRepository;
            _stateProvider = stateProvider;
            _dateTime = dateTime;
        }
        /// <summary>
        /// Возвращает информацию о сообщении.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>EmailInfo</returns>
        public async Task<EmailInfo> Invoke(Email email)
        {
            _latestState = await _stateProvider.GetAsync();
            if (ConsumeCounter()) return await _emailService.Send(email);

            if (!ConsumeCounter())
            {
                email.SetEmailInfo();
                email.SetState(EmailStatus.Query);
                await _emailRepository.InsertAsync(email);
                return email.Info;
            }

            return new EmailInfo
            {
                Date = _dateTime.Now.DateTime,
                UniversalId = Guid.Empty
            };
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