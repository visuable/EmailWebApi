using System.Threading.Tasks;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Entities.Dto;

namespace EmailWebApi.Services.Interfaces
{
    /// <summary>
    ///     Сервис статистики приложения.
    /// </summary>
    public interface IStatusService
    {
        /// <summary>
        ///     Возвращает информацию о сообщении.
        /// </summary>
        /// <param name="info"></param>
        /// <returns>EmailState</returns>
        Task<EmailState> GetEmailState(EmailInfo info);

        /// <summary>
        ///     Возвращает общую статистику приложения.
        /// </summary>
        /// <returns>ApplicationStateDto</returns>
        Task<ApplicationStateDto> GetApplicationState();
    }
}