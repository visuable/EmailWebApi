using EmailWebApi.Objects;
using Microsoft.Extensions.Logging;

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

        public ApplicationState GetApplicationState()
        {
            _logger.LogInformation("Возвращен статус приложения");
            return new ApplicationState
            {
                Total = _databaseService.GetCountByStatus(EmailStatus.None),
                Error = _databaseService.GetCountByStatus(EmailStatus.Error),
                Query = _databaseService.GetCountByStatus(EmailStatus.Query),
                Sent = _databaseService.GetCountByStatus(EmailStatus.Sent)
            };
        }

        public EmailState GetEmailState(EmailInfo info)
        {
            _logger.LogInformation("Получена информация по сообщению");
            return _databaseService.GetEmailByEmailInfo(info).State;
        }
    }
}