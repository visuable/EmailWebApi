using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWebApi.Objects;

namespace EmailWebApi.Services
{
    public interface IDatabaseManagerService
    {
        Task AddEmail(Email email);
        Task UpdateEmail(Email email);
        Task AddThrottlingState(ThrottlingState state);
        Task<Email> GetEmailByEmailInfo(EmailInfo info);
        Task<Email> GetEmailById(int id);
        Task<int> GetCountByStatus(EmailStatus status);
        Task<int> GetAllCount();
        Task<ThrottlingState> GetLastThrottlingState();
        Task<List<Email>> GetEmailsByStatus(EmailStatus status);
    }
}