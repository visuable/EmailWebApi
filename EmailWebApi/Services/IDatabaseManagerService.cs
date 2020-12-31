using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Entities.Dto;

namespace EmailWebApi.Services
{
    public interface IDatabaseManagerService
    {
        Task AddEmailAsync(Email email);
        Task UpdateEmailAsync(Email email);
        Task<Email> GetEmailByEmailInfoAsync(EmailInfo info);
        Task<int> GetCountByStatusAsync(EmailStatus status);
        Task<int> GetAllCountAsync();
        Task<ThrottlingStateDto> GetThrottlingStateAsync();
        Task<List<Email>> GetEmailsByStatusAsync(EmailStatus status);
    }
}