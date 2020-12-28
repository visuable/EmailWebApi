using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using EmailWebApi.Extensions;
using EmailWebApi.Objects;
using EmailWebApi.Objects.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EmailWebApi.Services
{
    public class EmailTransferService : IEmailTransferService
    {
        private readonly ILogger<EmailTransferService> _logger;
        private readonly IDatabaseManagerService _manager;
        private readonly IOptions<SmtpSettings> _options;

        private readonly SmtpClient _client;

        public EmailTransferService(IDatabaseManagerService manager, IOptions<SmtpSettings> options,
            ILogger<EmailTransferService> logger)
        {
            _manager = manager;
            _options = options;
            _logger = logger;
            var value = _options.Value;
            _client = new SmtpClient
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
        }

        public async Task<EmailInfo> Send(Email email)
        {
            return await SmtpSendMessage(email);
        }

        private async Task<EmailInfo> SmtpSendMessage(Email email)
        {
            try
            {
                await _client.SendMailAsync(_options.Value.SenderEmail, email.Content.Address, email.Content.Title,
                    email.Content.Body.Body);
                _logger.LogInformation("Сообщение отправлено");
                email.SetState(EmailStatus.Sent);
                _logger.LogInformation("Статус сообщения -- отправлено");
            }
            catch
            {
                _logger.LogError("Сообщение не отправлено");
                email.SetState(EmailStatus.Error);
                _logger.LogError("Статус сообщения -- ошибка");
            }
            finally
            {
                email.SetEmailInfo();
                _logger.LogInformation("EmailInfo установлено для данного сообщения");
                if (email.Id == 0)
                {
                    await _manager.AddEmail(email);
                }
                else
                {
                    await _manager.UpdateEmail(email);
                }
            }
            return email.Info;
        }
    }
}