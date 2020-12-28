using EmailWebApi.Objects;
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

        public async Task<ApplicationState> GetApplicationState()
        {
            _logger.LogInformation("Возвращен статус приложения");
            return new ApplicationState
            {
                Total = await _databaseService.GetAllCount(),
                Error = await _databaseService.GetCountByStatus(EmailStatus.Error),
                Query = await _databaseService.GetCountByStatus(EmailStatus.Query),
                Sent = await _databaseService.GetCountByStatus(EmailStatus.Sent)
            };
        }

        public async Task<EmailState> GetEmailState(EmailInfo info)
        {
            _logger.LogInformation("Получена информация по сообщению");
            var result = await _databaseService.GetEmailByEmailInfo(info);
            if (result == null)
            {
                _logger.LogError("Сообщение не найдено");
                return new EmailState();
            }
            else
            {
                return result.State;
            }
        }
    }
}