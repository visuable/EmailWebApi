using System.Threading.Tasks;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Repositories;
using EmailWebApi.Extensions;
using EmailWebApi.Services.Interfaces;

namespace EmailWebApi.Tests.Fakes
{
    public class FakeEmailTransferService : IEmailTransferService
    {
        private readonly IRepository<Email> _repository;

        public FakeEmailTransferService(IRepository<Email> repository)
        {
            _repository = repository;
        }

        public async Task<EmailInfo> Send(Email email)
        {
            email.SetState(EmailStatus.Sent);
            email.SetEmailInfo();
            await _repository.InsertAsync(email);
            return email.Info;
        }
    }
}