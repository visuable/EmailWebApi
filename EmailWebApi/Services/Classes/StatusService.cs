using System.Threading.Tasks;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Repositories;
using EmailWebApi.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace EmailWebApi.Services.Classes
{
    /// <summary>
    ///     Сервис статистики приложения.
    /// </summary>
    public class StatusService : IStatusService
    {
        private readonly ILogger<StatusService> _logger;
        private readonly IRepository<Email> _repository;

        public StatusService(ILogger<StatusService> logger, IRepository<Email> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        /// <summary>
        ///     Возвращает общую статистику приложения.
        /// </summary>
        /// <remarks>По умолчанию значения равны 0.</remarks>
        /// <returns>ApplicationStateDto</returns>
        public async Task<ApplicationState> GetApplicationState()
        {
            var applicationState = new ApplicationState();
            try
            {
                applicationState = new ApplicationState
                {
                    Total = await _repository.GetCountAsync(),
                    Error = await _repository.GetCountAsync(x => x.State.Status == EmailStatus.Error),
                    Query = await _repository.GetCountAsync(x => x.State.Status == EmailStatus.Query),
                    Sent = await _repository.GetCountAsync(x => x.State.Status == EmailStatus.Sent)
                };
            }
            catch
            {
                _logger.LogError("Ошибка получения данных с сервера");
            }

            return applicationState;
        }

        /// <summary>
        ///     Возвращает информацию о сообщении.
        /// </summary>
        /// <param name="info"></param>
        /// <returns>EmailState</returns>
        public async Task<EmailState> GetEmailState(EmailInfo info)
        {
            var result = new EmailState();
            try
            {
                result = (await _repository.FirstAsync(x =>
                    x.Info.UniversalId == info.UniversalId || x.Info.Date == info.Date)).State;
            }
            catch
            {
                _logger.LogError($"Невозможно получить EmailState по Guid {info.UniversalId}");
            }

            return result;
        }
    }
}