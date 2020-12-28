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
            try
            {
                await _client.SendMailAsync(_options.Value.SenderEmail, email.Content.Address, email.Content.Title,
                    email.Content.Body.Body);
                _logger.LogDebug($"Сообщение {email.Id} отправлено");
                email.SetState(EmailStatus.Sent);
            }
            catch
            {
                _logger.LogError($"Сообщение {email.Id} не отправлено");
                email.SetState(EmailStatus.Error);
            }
            finally
            {
                email.SetEmailInfo();
                _logger.LogDebug($"EmailInfo установлено для сообщения {email.Id}");
                _logger.LogDebug($"Статус сообщения {email.State.Status}");
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