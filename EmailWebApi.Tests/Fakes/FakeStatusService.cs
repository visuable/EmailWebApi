using System.Threading.Tasks;
using EmailWebApi.Db.Entities;
using EmailWebApi.Services.Interfaces;

namespace EmailWebApi.Tests.Fakes
{
    public class FakeStatusService : IStatusService
    {
        public Task<EmailState> GetEmailState(EmailInfo info)
        {
            return Task.FromResult(new EmailState
            {
                Status = EmailStatus.Sent
            });
        }

        public Task<ApplicationState> GetApplicationState()
        {
            return Task.FromResult(new ApplicationState
            {
                Error = 0,
                Query = 0,
                Sent = 0,
                Total = 0
            });
        }
    }
}