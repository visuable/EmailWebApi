using EmailWebApi.Objects;
using EmailWebApi.Objects.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EmailWebApi.Services
{
    public class EmailTransferService : IEmailTransferService
    {
        private IDatabaseManagerService _manager;
        private IOptions<SmtpSettings> _options;
        private ILogger<EmailTransferService> _logger;

        private SmtpClient client;
        public EmailTransferService(IDatabaseManagerService manager, IOptions<SmtpSettings> options, ILogger<EmailTransferService> logger)
        {
            _manager = manager;
            _options = options;
            _logger = logger;
            var value = _options.Value;
            client = new SmtpClient()
            {
                Credentials = new NetworkCredential()
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
            if(email.State.Status == EmailStatus.Query)
            {
                if(email.Id == 0)
                {
                    _logger.LogError("Ошибка троттлинг функции");
                    throw new Exception();
                }
                else
                {
                    email.Info = new EmailInfo()
                    {
                        Date = DateTime.Now,
                        UniversalId = Guid.NewGuid()
                    };
                    _manager.AddEmail(email);
                    return email;
                }
            }
            else if(email.State.Status == EmailStatus.None)
            {
                if(email.Id == 0)
                {
                    email.Info = new EmailInfo()
                    {
                        UniversalId = Guid.NewGuid(),
                        Date = DateTime.Now
                    };
                    _manager.AddEmail(email);
                    return SendEmail(email);
                }
                else
                {
                    email = _manager.GetEmailById(email.Id);
                    if(email == null)
                    {
                        _logger.LogError("Ошибка получения сообщения");
                        throw new Exception();
                    }
                    else
                    {
                        if(email.State.Status == EmailStatus.Query)
                        {
                            return SendEmail(email);
                        }
                        else
                        {
                            _logger.LogError("Ошибка валидации сообщения");
                            throw new Exception();
                        }
                    }
                }
            }
            else
            {
                return email;
            }
        }
        private Email SendEmail(Email email)
        {
            try
            {
                client.Send(_options.Value.SenderEmail, email.Content.Address, email.Content.Title, email.Content.Body.Body);
                email.Info.Date = DateTime.Now;
                email.State.Status = EmailStatus.Sent;
                _logger.LogInformation("Сообщение отправлено");
            }
            catch
            {
                email.Info.Date = DateTime.Now;
                email.State.Status = EmailStatus.Error;
            }
            _manager.UpdateEmail(email);
            return email;
        }
    }
}
