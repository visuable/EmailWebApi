using System.Threading.Tasks;
using EmailWebApi.Db.Entities;

namespace EmailWebApi.Services.Interfaces
{
    /// <summary>
    ///     Сервис отправки сообщений.
    /// </summary>
    public interface IEmailTransferService
    {
        /// <summary>
        ///     Отсылает сообщение и устанавливает его статус.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>EmailInfo</returns>
        Task<EmailInfo> Send(Email email);
    }
}