using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailWebApi.Db.Entities;
using EmailWebApi.Services;

namespace EmailWebApi.Tests.Fakes
{
    public class FakeEmailTransferService : IEmailTransferService
    {
        public Task<EmailInfo> Send(Email email)
        {
            return Task.FromResult(new EmailInfo()
            {
                Date = DateTime.Now,
                UniversalId = Guid.NewGuid()
            });
        }
    }
}
