using System.Threading.Tasks;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Entities.Settings;
using EmailWebApi.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace EmailWebApi.Services.Classes
{
    /// <summary>
    ///     Сервис-фабрика SmtpClient.
    /// </summary>
    public class SmtpSenderService : ISmtpSenderService
    {
        private readonly IOptions<SmtpSettings> _options;
        private readonly ISmtpClientFactoryService _smtpClientFactory;

        public SmtpSenderService(ISmtpClientFactoryService smtpClientFactory, IOptions<SmtpSettings> options)
        {
            _smtpClientFactory = smtpClientFactory;
            _options = options;
        }

        /// <summary>
        ///     Отправляет сообщение по Smtp протоколу.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        //TODO: требует интеграционного теста. 
        public async Task SendAsync(Email email)
        {
            var client = _smtpClientFactory.Create();
            await client.SendMailAsync(_options.Value.SenderEmail, email.Content.Address, email.Content.Title,
                email.Content.Body.Body);
        }
    }
}