using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWebApi.Entities;

namespace EmailWebApi.Services
{
    public interface IDatabaseManagerService
    {
        Task AddEmailAsync(Email email);
        Task UpdateEmailAsync(Email email);
        Task AddThrottlingStateAsync(ThrottlingState state);
        Task<Email> GetEmailByEmailInfoAsync(EmailInfo info);
        Task<int> GetCountByStatusAsync(EmailStatus status);
        Task<int> GetAllCountAsync();
        Task<ThrottlingState> GetLastThrottlingStateAsync();
        Task<List<Email>> GetEmailsByStatusAsync(EmailStatus status);
    }
}