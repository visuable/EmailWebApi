using System.Threading.Tasks;
using EmailWebApi.Db.Entities;
using EmailWebApi.Services.Interfaces;

namespace EmailWebApi.Tests.Fakes
{
    public class FakeSmtpSenderService : ISmtpSenderService
    {
        public Task SendAsync(Email email)
        {
            return Task.CompletedTask;
        }
    }
}