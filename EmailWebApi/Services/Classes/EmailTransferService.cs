using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Entities.Settings;
using EmailWebApi.Db.Repositories;
using EmailWebApi.Extensions;
using EmailWebApi.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EmailWebApi.Services.Classes
{
    /// <summary>
    /// Сервис отправки сообщений.
    /// </summary>
    public class EmailTransferService : IEmailTransferService
    {
        private readonly ILogger<EmailTransferService> _logger;
        private readonly ISmtpClientFactoryService _smtpClientFactory;
        private readonly IRepository<Email> _repository;
        private readonly IOptions<SmtpSettings> _options;

        public EmailTransferService(IRepository<Email> repository, ISmtpClientFactoryService smtpClientFactory,
            ILogger<EmailTransferService> logger, IOptions<SmtpSettings> options)
        {
            _repository = repository;
            _smtpClientFactory = smtpClientFactory;
            _logger = logger;
            _options = options;
        }
        /// <summary>
        /// Отсылает сообщение и устанавливает его статус.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>EmailInfo</returns>
        public async Task<EmailInfo> Send(Email email)
        {
            try
            {
                var client = _smtpClientFactory.Create();
                await client.SendMailAsync(_options.Value.SenderEmail, email.Content.Address, email.Content.Title, email.Content.Body.Body);
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
                    await _repository.InsertAsync(email);
                else
                    await _repository.UpdateAsync(email);
            }

            return email.Info;
        }
    }
}