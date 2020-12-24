using EmailWebApi.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        List<Email> GetEmailsByStatus(EmailStatus status);
    }
}
