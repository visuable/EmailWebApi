using EmailWebApi.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Services
{
    public class StatusService : IStatusService
    {
        private ILogger<StatusService> _logger;
        private IDatabaseManagerService _databaseService;
        public StatusService(ILogger<StatusService> logger, IDatabaseManagerService databaseService)
        {
            _logger = logger;
            _databaseService = databaseService;
        }
        public ApplicationState GetApplicationState()
        {
            _logger.LogInformation("Возвращен статус приложения");
            return new ApplicationState()
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
