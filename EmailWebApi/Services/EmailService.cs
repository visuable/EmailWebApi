using AutoMapper;
using EmailWebApi.Models;
using EmailWebApi.Models.Dto;
using EmailWebApi.Settings;
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
    public class EmailService : IEmailService
    {
        private ILogger<EmailService> _logger;
        private IMapper _mapper;
        private IDatabaseManager _manager;
        private IOptions<SmtpSettings> _settings;

        private SmtpClient client;
        public EmailService(ILogger<EmailService> logger, IMapper mapper, IDatabaseManager manager, IOptions<SmtpSettings> settings)
        {
            _logger = logger;
            _mapper = mapper;
            _manager = manager;
            _settings = settings;

            client = new SmtpClient()
            {
                Port = _settings.Value.Port,
                Host = _settings.Value.Host,
                Credentials = new NetworkCredential()
                {
                    UserName = _settings.Value.Username,
                    Password = _settings.Value.Password
                },
                EnableSsl = true,
                UseDefaultCredentials = false,
            };
        }
        public StatusModel GetCurrentStatus()
        {
            var model = new StatusModel()
            {
                Total = _manager.GetAllCount(),
                Failed = _manager.GetCountByPredicate(x => x.IsArrived == false),
                Sent = _manager.GetCountByPredicate(x => x.IsArrived == true)
            };
            return model;
        }

        public Email GetEmailStatus(Guid id)
        {
            var item = new Email();
            try
            {
                item = _manager.Get(id);
                _logger.LogDebug($"Returned from database: {id}");
            }
            catch
            {
                _logger.LogError($"Error while search in database: {id}");
            }
            return item;
        }

        public async Task<Guid> SendEmail(Email email)
        {
            var id = Guid.NewGuid();
            var isArrived = false;
            try
            {
                await client.SendMailAsync(_settings.Value.SenderEmail, email.Adress, email.Title, email.Body);
                isArrived = true;
                _logger.LogInformation($"OK: {id}");
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed: {id}");
            }
            finally
            {
                var model = new EmailStatus()
                {
                    EmailId = id,
                    SentDate = DateTime.Now.ToShortDateString(),
                    SentTime = DateTime.Now.ToShortTimeString(),
                    IsArrived = isArrived,
                };
                email.EmailStatus = model;
                _manager.Insert(email);
            }
            return id;
        }
    }
}
