using System.Collections.Generic;
using EmailWebApi.Objects;

namespace EmailWebApi.Services
{
    public interface IDatabaseManagerService
    {
        void AddEmail(Email email);
        Email GetEmailByEmailInfo(EmailInfo info);
        int GetCountByStatus(string status);
        void UpdateEmail(Email email);
        Email GetEmailById(int id);
        ThrottlingState GetLastThrottlingState();
        void AddThrottlingState(ThrottlingState state);
        List<Email> GetEmailsByStatus(string status);
        string GetEmailStateById(int id);
    }
}