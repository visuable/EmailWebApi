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

        public async Task Send(Email email)
        {
            await SmtpSendMessage(email);
        }

        private async Task SmtpSendMessage(Email email)
        {
            try
            {
                await _client.SendMailAsync(_options.Value.SenderEmail, email.Content.Address, email.Content.Title,
                    email.Content.Body.Body);
                email.SetState(EmailStatus.Sent);
            }
            catch
            {
                email.SetState(EmailStatus.Error);
            }
            finally
            {
                email.SetEmailInfo();
                if (email.Id == 0)
                {
                    _manager.AddEmail(email);
                }
                else
                {
                    _manager.UpdateEmail(email);
                }
            }
        }
    }
}