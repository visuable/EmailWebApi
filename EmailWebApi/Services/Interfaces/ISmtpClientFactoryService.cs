using System.Net.Mail;

namespace EmailWebApi.Services.Interfaces
{
    /// <summary>
    ///     Создает экземпляр SmtpClient с заданными настройками.
    /// </summary>
    public interface ISmtpClientFactoryService
    {
        /// <summary>
        ///     Создает SmtpClient.
        /// </summary>
        /// <returns>SmtpClient</returns>
        SmtpClient Create();
    }
}