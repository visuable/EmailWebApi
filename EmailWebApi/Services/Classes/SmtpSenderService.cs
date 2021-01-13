using System;
using System.Threading.Tasks;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Entities.Settings;
using EmailWebApi.Services.Interfaces;
using MailKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace EmailWebApi.Services.Classes
{
    /// <summary>
    ///     Сервис-фабрика SmtpClient.
    /// </summary>
    public class SmtpSenderService : ISmtpSenderService
    {
        private readonly IOptions<SmtpSettings> _options;
        private readonly ISmtpClientFactoryService _clientFactory;

        public SmtpSenderService(IOptions<SmtpSettings> options, ISmtpClientFactoryService clientFactory)
        {
            _options = options;
            _clientFactory = clientFactory;
        }

        /// <summary>
        ///     Отправляет сообщение по Smtp протоколу.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        //TODO: требует интеграционного теста. 
        public async Task SendAsync(Email email)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_options.Value.SenderName, _options.Value.SenderEmail));
            message.To.Add(InternetAddress.Parse(email.Content.Address));
            message.Body = new TextPart(Enum.Parse<TextFormat>(_options.Value.TextType))
            {
                Text = email.Content.Body.Body
            };
            message.Subject = email.Content.Title;

            var client = await _clientFactory.CreateAsync();
            await client.ConnectAsync(_options.Value.Host, _options.Value.Port);
            await client.AuthenticateAsync(_options.Value.Username, _options.Value.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(false);
        }
    }
}