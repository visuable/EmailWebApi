using System;
using System.Threading.Tasks;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Repositories;
using EmailWebApi.Extensions;
using EmailWebApi.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace EmailWebApi.Services.Classes
{
    /// <summary>
    ///     Сервис отправки сообщений.
    /// </summary>
    public class EmailTransferService : IEmailTransferService
    {
        private readonly ILogger<EmailTransferService> _logger;
        private readonly IRepository<Email> _repository;
        private readonly ISmtpSenderService _smtpSenderService;

        public EmailTransferService(IRepository<Email> repository, ISmtpSenderService smtpSenderService,
            ILogger<EmailTransferService> logger)
        {
            _repository = repository;
            _smtpSenderService = smtpSenderService;
            _logger = logger;
        }

        /// <summary>
        ///     Отсылает сообщение и устанавливает его статус.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>EmailInfo</returns>
        public async Task<EmailInfo> Send(Email email)
        {
            try
            {
                await _smtpSenderService.SendAsync(email);
                email.SetState(EmailStatus.Sent);
            }
            catch
            {
                email.SetState(EmailStatus.Error);
            }
            finally
            {
                email.SetEmailInfo();
                _logger.LogDebug($"EmailInfo установлено для сообщения {email.Id}");
                _logger.LogDebug($"Статус сообщения {email.State.Status}");
                //TODO: что-то сделать с email.Id == 0.
                if (email.Id == 0)
                {
                    if (!email.Content.Body.Save) email.Content.Body.Body = string.Empty;
                    await _repository.InsertAsync(email);
                }
                else
                    await _repository.UpdateAsync(email);
            }

            return email.Info;
        }
    }
}