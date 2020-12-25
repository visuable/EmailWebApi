using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailWebApi.Database;
using EmailWebApi.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EmailWebApi.Services
{
    public class DatabaseManagerService : IDatabaseManagerService
    {
        private readonly ILogger<DatabaseManagerService> _logger;
        private readonly EmailContext _context;

        public DatabaseManagerService(IServiceScopeFactory scope)
        {
            var scoped = scope.CreateScope();
            _context = scoped.ServiceProvider.GetRequiredService<EmailContext>();
            _logger = scoped.ServiceProvider.GetRequiredService<ILogger<DatabaseManagerService>>();
        }

        public void AddEmail(Email email)
        {
            try
            {
                if (email.Content.Body.Save)
                {
                    _context.Emails.Add(email);
                }
                else
                {
                    email.Content.Body.Body = string.Empty;
                    _context.Emails.Add(email);
                }

                _context.SaveChanges();
            }
            catch
            {
                _logger.LogError("Сообщение не может быть внесено в базу данных");
            }

            _logger.LogInformation("Сообщение внесено в базу данных");
        }

        public Email GetEmailByEmailInfo(EmailInfo info)
        {
            return _context.Emails.ToList()
                .FirstOrDefault(x => x.Info.UniversalId == info.UniversalId || x.Info.Date == info.Date);
        }

        public int GetCountByStatus(EmailStatus status)
        {
            return _context.Emails.Where(x => x.State.Status == status).ToList().Count;
        }

        public void UpdateEmail(Email email)
        {
            try
            {
                _context.Emails.Update(email);
                _context.SaveChanges();
                _logger.LogInformation("Сообщение обновилось");
            }
            catch
            {
                _logger.LogError("Сообщение не обновилось");
            }
        }

        public Email GetEmailById(int id)
        {
            return _context.Emails.ToList().First(x => x.Id == id);
        }

        public ThrottlingState GetLastThrottlingState()
        {
            return _context.ThrottlingStates.ToList().OrderByDescending(x => x.Id).ToList().FirstOrDefault();
        }

        public void AddThrottlingState(ThrottlingState state)
        {
            try
            {
                _context.ThrottlingStates.Add(state);
                _context.SaveChanges();
            }
            catch
            {
                _logger.LogError("Состояние запросов не может быть зафиксировано");
            }

            _logger.LogInformation("Состояние запросов зафиксировано");
        }

        public async Task<List<Email>> GetEmailsByStatus(EmailStatus status)
        {
            return (await _context.Emails.ToListAsync()).Where(x => x.State.Status == status).ToList();
        }

        public List<Email> GetAll()
        {
            return _context.Emails.ToList();
        }
    }
}