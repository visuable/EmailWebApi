using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using EmailWebApi.Db.Entities.Settings;
using EmailWebApi.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace EmailWebApi.Services.Classes
{
    /// <summary>
    /// Создает экземпляр SmtpClient с заданными настройками.
    /// </summary>
    public class SmtpClientFactoryService : ISmtpClientFactoryService
    {
        private readonly IOptions<SmtpSettings> _options;

        public SmtpClientFactoryService(IOptions<SmtpSettings> options)
        {
            _options = options;
        }
        /// <summary>
        /// Создает SmtpClient.
        /// </summary>
        /// <returns>SmtpClient</returns>
        public SmtpClient Create()
        {
            var value = _options.Value;
            var client = new SmtpClient
            {
                Credentials = new NetworkCredential
                {
                    UserName = value.Username,
                    Password = value.Password
                },
                EnableSsl = true,
                Host = value.Host,
                Port = value.Port
            };
            return client;
        }
    }
}
