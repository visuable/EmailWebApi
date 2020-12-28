using EmailWebApi.Objects;
using EmailWebApi.Objects.Dto;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EmailWebApi.Services
{
    public class StatusService : IStatusService
    {
        private readonly IDatabaseManagerService _databaseService;
        private readonly ILogger<StatusService> _logger;

        public StatusService(ILogger<StatusService> logger, IDatabaseManagerService databaseService)
        {
            _logger = logger;
            _databaseService = databaseService;
        }

        public async Task<ApplicationStateDto> GetApplicationState()
        {
            var applicationState = new ApplicationStateDto();
            try
            {
                applicationState = new ApplicationStateDto
                {
                    Total = await _databaseService.GetAllCount(),
                    Error = await _databaseService.GetCountByStatus(EmailStatus.Error),
                    Query = await _databaseService.GetCountByStatus(EmailStatus.Query),
                    Sent = await _databaseService.GetCountByStatus(EmailStatus.Sent)
                };
                _logger.LogDebug("Возвращен статус приложения");
            }
            catch
            {
                _logger.LogError("Ошибка получения данных с сервера");
            }
            return applicationState;
        }

        public async Task<EmailState> GetEmailState(EmailInfo info)
        {
            var result = new EmailState();
            try
            {
                result = (await _databaseService.GetEmailByEmailInfo(info)).State;
                _logger.LogDebug("Получена информация по сообщению");
            }
            catch
            {
                _logger.LogError($"Невозможно получить EmailState по Guid {info.UniversalId}");
            }
            return result;
        }
    }
}