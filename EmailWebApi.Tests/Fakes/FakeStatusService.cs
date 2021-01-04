using System.Threading.Tasks;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Entities.Dto;
using EmailWebApi.Services;
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

        public Task<ApplicationStateDto> GetApplicationState()
        {
            return Task.FromResult(new ApplicationStateDto
            {
                Error = -1,
                Query = -1,
                Sent = -1,
                Total = -1
            });
        }
    }
}