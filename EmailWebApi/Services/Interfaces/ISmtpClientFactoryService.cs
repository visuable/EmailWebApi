using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;

namespace EmailWebApi.Services.Interfaces
{
    /// <summary>
    /// Создает SmtpClient.
    /// </summary>
    public interface ISmtpClientFactoryService
    {
        /// <summary>
        /// Создает SmtpClient с заданными настройками.
        /// </summary>
        /// <returns>Экземпляр ISmtpClient</returns>
        Task<ISmtpClient> CreateAsync();
    }
}
