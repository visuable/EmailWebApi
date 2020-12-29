using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailWebApi.Database;
using EmailWebApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmailWebApi.Services
{
    public class DatabaseManagerService : IDatabaseManagerService
    {
        private readonly EmailContext _context;
        private readonly ILogger<DatabaseManagerService> _logger;

        public DatabaseManagerService(ILogger<DatabaseManagerService> logger, EmailContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task AddEmailAsync(Email email)
        {
            var copy = new Email
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

                _context.SaveChanges();
            }
            catch
            {
                _logger.LogError("Сообщение не может быть внесено в базу данных");
            }
        }

        public async Task<Email> GetEmailByEmailInfoAsync(EmailInfo info)
        {
            Email email = null;
            try
            {
                email = await _context.Emails.FirstOrDefaultAsync(x =>
                    x.Info.Date == info.Date || x.Info.UniversalId == info.UniversalId);
                _logger.LogDebug(
                    $"Сообщение {email.Id} возвращено из базы данных по идентификатору EmailInfo {info.Id}");
            }
            catch
            {
                _logger.LogError($"Сообщения с указанным Guid {info.UniversalId}  нет в базе данных");
            }

            return email;
        }

        public async Task<int> GetCountByStatusAsync(EmailStatus status)
        {
            var count = 0;
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

        public async Task UpdateEmailAsync(Email email)
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

        public async Task<ThrottlingState> GetLastThrottlingStateAsync()
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

        public async Task AddThrottlingStateAsync(ThrottlingState state)
        {
            try
            {
                await _context.ThrottlingStates.AddAsync(state);
                _context.SaveChanges();
                _logger.LogDebug($"Состояние {state.Id} внесено в базу данных");
            }
            catch
            {
                _logger.LogError("Указанное состояние запросов не может быть внесено в базу данных");
            }
        }

        public async Task<List<Email>> GetEmailsByStatusAsync(EmailStatus status)
        {
            var emails = new List<Email>();
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

        public async Task<int> GetAllCountAsync()
        {
            var count = 0;
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