using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Entities.Dto;
using EmailWebApi.Db.Repositories;
using EmailWebApi.Extensions;
using Microsoft.Extensions.Logging;

namespace EmailWebApi.Services
{
    public class DatabaseManagerService : IDatabaseManagerService
    {
        private readonly ILogger<DatabaseManagerService> _logger;
        private readonly IEmailRepository _repository;

        public DatabaseManagerService(ILogger<DatabaseManagerService> logger, IEmailRepository repository)
        {
            _logger = logger;
            _repository = repository;
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
                    await _repository.AddAsync(email);
                    _logger.LogDebug($"Сообщение {email.Id} внесено в базу данных с Body");
                }
                else
                {
                    copy.Content.Body.Body = string.Empty;
                    await _repository.AddAsync(email);
                    _logger.LogDebug($"Сообщение {email.Id} внесено в базу данных без Body");
                }

                _repository.SaveChanges();
            }
            catch
            {
                _logger.LogError("Сообщение не может быть внесено в базу данных");
            }
        }

        public async Task<Email> GetEmailByEmailInfoAsync(EmailInfo info)
        {
            var email = new Email();
            try
            {
                email = _repository.GetListByPredicate(x =>
                    x.Info.UniversalId == info.UniversalId || x.Info.Date == info.Date).FirstOrDefault();
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
                count = await _repository.CountAsync(x => x.State.Status == status);
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
                _repository.Update(email);
                await _repository.SaveChangesAsync();
                _logger.LogDebug($"Сообщение {email.Id} обновилось в базе данных");
            }
            catch
            {
                _logger.LogError("Сообщение не обновилось в базе данных");
            }
        }

        public async Task<ThrottlingStateDto> GetThrottlingStateAsync()
        {
            var stateDto = new ThrottlingStateDto();
            try
            {
                var offset = DateTimeOffset.Now.AddSeconds(-60);
                var first = await _repository.GetFirstAsync();
                stateDto.Counter = await _repository.CountAsync(x => x.Info.Date >= offset);
                stateDto.LastAddress = first.Content.Address;
                stateDto.LastAddressCounter = await _repository.CountAsync(x => x.Info.Date >= offset &&
                                                                                x.Content.Address ==
                                                                                stateDto.LastAddress);
                stateDto.EndPoint = DateTime.Now;
                _logger.LogDebug("Извлечено последнее состояние");
            }
            catch
            {
                stateDto.Refresh();
                _logger.LogError("Ошибка при получении последнего состояния");
            }

            return stateDto;
        }

        public async Task<List<Email>> GetEmailsByStatusAsync(EmailStatus status)
        {
            var emails = new List<Email>();
            try
            {
                emails = (await _repository.GetListByPredicateAsync(x => x.State.Status == status)).ToList();
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
                count = await _repository.CountAsync();
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