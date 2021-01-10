using System.Net;
using System.Net.Mail;
using EmailWebApi.Db.Entities.Settings;
using EmailWebApi.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EmailWebApi.Services.Classes
{
    /// <summary>
    ///     Создает экземпляр SmtpClient с заданными настройками.
    /// </summary>
    public class SmtpClientFactoryService : ISmtpClientFactoryService
    {
        private readonly ILogger<SmtpClientFactoryService> _logger;
        private readonly IOptions<SmtpSettings> _options;

        public SmtpClientFactoryService(IOptions<SmtpSettings> options, ILogger<SmtpClientFactoryService> logger)
        {
            _options = options;
            _logger = logger;
        }

        /// <summary>
        ///     Создает SmtpClient.
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
            _logger.LogDebug("Создан экземпляр SmtpClient");
            return client;
        }
    }
}