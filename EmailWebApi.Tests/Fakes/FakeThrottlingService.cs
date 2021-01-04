using System;
using System.Threading.Tasks;
using EmailWebApi.Db.Entities;
using EmailWebApi.Services;
using EmailWebApi.Services.Interfaces;

namespace EmailWebApi.Tests.Fakes
{
    public class FakeThrottlingService : IThrottlingService
    {
        public Task<EmailInfo> Invoke(Email email)
        {
            return Task.FromResult(new EmailInfo
            {
                Date = DateTime.Now,
                Id = 0,
                UniversalId = Guid.Empty
            });
        }
    }
}