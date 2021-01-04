using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EmailWebApi.Services.Interfaces
{
    /// <summary>
    /// Создает экземпляр SmtpClient с заданными настройками.
    /// </summary>
    public interface ISmtpClientFactoryService
    {
        /// <summary>
        /// Создает SmtpClient.
        /// </summary>
        /// <returns>SmtpClient</returns>
        SmtpClient Create();
    }
}
