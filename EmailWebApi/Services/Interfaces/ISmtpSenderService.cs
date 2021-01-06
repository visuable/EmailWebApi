using System.Threading.Tasks;
using EmailWebApi.Db.Entities;

namespace EmailWebApi.Services.Interfaces
{
    /// <summary>
    ///     Сервис-фабрика SmtpClient.
    /// </summary>
    public interface ISmtpSenderService
    {
        /// <summary>
        ///     Отправляет сообщение по Smtp протоколу.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task SendAsync(Email email);
    }
}