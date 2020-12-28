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

        public async Task AddEmail(Email email)
        {
            var copy = new Email()
            {
                Content = email.Content,
                Id = email.Id,
                Info = email.Info,
                State = email.State
            };
            try
            {
                if (copy.Content.Body.Save)
                {
                    await _context.Emails.AddAsync(email);
                    _logger.LogDebug($"Сообщение {email.Id} внесено в базу данных с Body");
                }
                else
                {
                    copy.Content.Body.Body = string.Empty;
                    await _context.Emails.AddAsync(email);
                    _logger.LogDebug($"Сообщение {email.Id} внесено в базу данных без Body");
                }

                await _context.SaveChangesAsync();
            }
            catch
            {
                _logger.LogError($"Сообщение не может быть внесено в базу данных");
            }

            
        }

        public async Task<Email> GetEmailByEmailInfo(EmailInfo info)
        {
            Email email = null;
            try
            {
                email = await _context.Emails.FirstOrDefaultAsync(x => x.Info.Date == info.Date || x.Info.UniversalId == info.UniversalId);
                _logger.LogDebug($"Сообщение {email.Id} возвращено из базы данных по идентификатору EmailInfo {info.Id}");
            }
            catch
            {
                _logger.LogError($"Сообщения с указанным Guid {info.UniversalId}  нет в базе данных");
            }
            return email;
        }

        public async Task<int> GetCountByStatus(EmailStatus status)
        {
            int count = 0;
            try
            {
                count = await _context.Emails.CountAsync(x => x.State.Status == status);
                _logger.LogDebug($"Возвращено {count} записей в базе данных");
            }
            catch
            {
                _logger.LogError("Ошибка при выполнении COUNT");
            }
            return count;
        }

        public async Task UpdateEmail(Email email)
        {
            try
            {
                _context.Emails.Update(email);
                await _context.SaveChangesAsync();
                _logger.LogDebug($"Сообщение {email.Id} обновилось в базе данных");
            }
            catch
            {
                _logger.LogError("Сообщение не обновилось в базе данных");
            }
        }

        public async Task<Email> GetEmailById(int id)
        {
            Email email = null;
            try
            {
                email =  await _context.Emails.FindAsync(id);
                _logger.LogDebug($"Сообщение извлечено из базы данных по идентификатору {id}");
            }
            catch
            {
                _logger.LogError($"Сообщение под идентификатором {id} не найдено в базе данных");
            }
            return email;
        }

        public async Task<ThrottlingState> GetLastThrottlingState()
        {
            ThrottlingState state = null;
            try
            {
                state = await _context.ThrottlingStates.OrderByDescending(x => x.EndPoint).FirstOrDefaultAsync();
                _logger.LogDebug($"Извлечено последнее состояние с идентификатором {state.Id}");
            }
            catch
            {
                _logger.LogError("Ошибка при получении последнего состояния");
            }
            return state;
        }

        public async Task AddThrottlingState(ThrottlingState state)
        {
            try
            {
                await _context.ThrottlingStates.AddAsync(state);
                await _context.SaveChangesAsync();
                _logger.LogDebug($"Состояние {state.Id} внесено в базу данных");
            }
            catch
            {
                _logger.LogError($"Указанное состояние запросов не может быть внесено в базу данных");
            }
        }

        public async Task<List<Email>> GetEmailsByStatus(EmailStatus status)
        {
            List<Email> emails = new List<Email>();
            try
            {
                emails = await _context.Emails.Where(x => x.State.Status == status).ToListAsync();
                _logger.LogDebug($"Получение списка сообщений со статусом {status}");
            }
            catch
            {
                _logger.LogError($"Ошибка при получении списка сообщений со статусом {status}");
            }
            return emails;
        }

        public async Task<int> GetAllCount()
        {
            int count = 0;
            try
            {
                count = await _context.Emails.CountAsync();
                _logger.LogDebug($"Получено количество всех записей {count}");
            }
            catch
            {
                _logger.LogError("Невозможно выполнить COUNT");
            }
            return count;
        }
    }
}