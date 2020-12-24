using System.Collections.Generic;
using System.Linq;
using EmailWebApi.Database;
using EmailWebApi.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmailWebApi.Services
{
    public class DatabaseManagerService : IDatabaseManagerService
    {
        private readonly ILogger<DatabaseManagerService> _logger;
        private readonly EmailContext context;

        public DatabaseManagerService(EmailContext context, ILogger<DatabaseManagerService> logger)
        {
            this.context = context;
            _logger = logger;
        }

        public void AddEmail(Email email)
        {
            try
            {
                if (email.Content.Body.Save)
                {
                    context.Emails.Add(email);
                }
                else
                {
                    email.Content.Body.Body = string.Empty;
                    context.Emails.Add(email);
                }

                context.SaveChanges();
            }
            catch
            {
                _logger.LogError("Сообщение не может быть внесено в базу данных");
            }

            _logger.LogInformation("Сообщение внесено в базу данных");
        }

        public void AddThrottlingState(ThrottlingState state)
        {
            try
            {
                context.ThrottlingStates.Add(state);
                context.SaveChanges();
            }
            catch
            {
                _logger.LogError("Состояние запросов не может быть зафиксировано");
            }

            _logger.LogInformation("Состояние запросов зафиксировано");
        }

        public int GetCountByStatus(string status)
        {
            return context.Emails.Where(x => x.State.Status == status).ToList().Count;
        }

        public Email GetEmailByEmailInfo(EmailInfo info)
        {
            return context.Emails.ToList()
                .FirstOrDefault(x => x.Info.UniversalId == info.UniversalId || x.Info.Date == info.Date);
        }

        public Email GetEmailById(int id)
        {
            return context.Emails.Find(id);
        }

        public List<Email> GetEmailsByStatus(string status)
        {
            return context.Emails.Include(x => x.State).Where(x => x.State.Status == status).ToList();

        }

        public string GetEmailStateById(int id)
        {
            return context.States.Find(id).Status;
        }

        public ThrottlingState GetLastThrottlingState()
        {
            return context.ThrottlingStates.OrderByDescending(x => x.Id).ToList().FirstOrDefault();
        }

        public void UpdateEmail(Email email)
        {
            try
            {
                context.Emails.Update(email);
                context.SaveChanges();
                _logger.LogInformation("Сообщение обновилось");
            }
            catch
            {
                _logger.LogError("Сообщение не обновилось");
            }
        }
    }
}