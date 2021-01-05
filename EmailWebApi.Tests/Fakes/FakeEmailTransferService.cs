using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Repositories;
using EmailWebApi.Extensions;
using EmailWebApi.Services;
using EmailWebApi.Services.Interfaces;

namespace EmailWebApi.Tests.Fakes
{
    public class FakeEmailTransferService : IEmailTransferService
    {
        private IRepository<Email> _repository;

        public FakeEmailTransferService(IRepository<Email> repository)
        {
            _repository = repository;
        }

        public async Task<EmailInfo> Send(Email email)
        {
            email.SetState(EmailStatus.Sent);
            await _repository.InsertAsync(email);
            return new EmailInfo()
            {
                Date = DateTime.Now,
                UniversalId = Guid.NewGuid()
            };
        }
    }
}
