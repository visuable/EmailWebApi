using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

        private readonly SmtpClient client;

        public EmailTransferService(IDatabaseManagerService manager, IOptions<SmtpSettings> options,
            ILogger<EmailTransferService> logger)
        {
            _manager = manager;
            _options = options;
            _logger = logger;
            var value = _options.Value;
            client = new SmtpClient
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

        public Email Send(Email email)
        {
            if (email.State.Status == EmailStatus.Query)
            {
                email.SetEmailInfo();
                _manager.AddEmail(email);
                return email;
            }

            if (email.State.Status == EmailStatus.None)
            {
                if (email.Id == 0)
                {
                    email.SetEmailInfo();
                    _manager.AddEmail(email);
                    return SendEmail(email);
                }
                var dbEmail = _manager.GetEmailsByStatus(EmailStatus.Query).FirstOrDefault(x => x.Id == email.Id)?.State.Status;
                if (dbEmail == EmailStatus.Query)
                {
                    return SendEmail(email);
                }

                _logger.LogError("Ошибка валидации сообщения");
                throw new Exception();
            }

            return email;
        }

        private Email SendEmail(Email email)
        {
            try
            {
                client.Send(_options.Value.SenderEmail, email.Content.Address, email.Content.Title,
                    email.Content.Body.Body);
                email.UpdateEmailDate();
                email.SetState(EmailStatus.Sent);
                _logger.LogInformation("Сообщение отправлено");
            }
            catch
            {
                email.UpdateEmailDate();
                email.SetState(EmailStatus.Error);
            }

            _manager.UpdateEmail(email);
            return email;
        }
    }
}