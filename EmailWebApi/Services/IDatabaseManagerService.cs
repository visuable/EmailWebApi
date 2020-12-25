using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWebApi.Objects;

namespace EmailWebApi.Services
{
    public interface IDatabaseManagerService
    {
        void AddEmail(Email email);
        Email GetEmailByEmailInfo(EmailInfo info);
        int GetCountByStatus(EmailStatus status);
        void UpdateEmail(Email email);
        Email GetEmailById(int id);
        ThrottlingState GetLastThrottlingState();
        void AddThrottlingState(ThrottlingState state);
        Task<List<Email>> GetEmailsByStatus(EmailStatus status);
        List<Email> GetAll();
    }
}