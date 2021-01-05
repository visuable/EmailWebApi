using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Repositories;

namespace EmailWebApi.Tests.Fakes
{
    public class FakeEmailRepository : IRepository<Email>
    {
        public FakeEmailRepository()
        {
            Emails = new List<Email>();
        }

        public List<Email> Emails { get; set; }

        public Task InsertAsync(Email item)
        {
            Emails.Add(item);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Email item)
        {
            var last = Emails.FirstOrDefault(x => x.Content.Body.Body == item.Content.Body.Body);
            Emails[last.Id] = item;
            return Task.CompletedTask;
        }

        public Task<Email> FirstAsync(Expression<Func<Email, bool>> predicate)
        {
            return Task.FromResult(Emails.FirstOrDefault(predicate.Compile()));
        }

        public Task<Email> FirstAsync()
        {
            return Task.FromResult(Emails.FirstOrDefault());
        }

        public Task<IEnumerable<Email>> GetAllAsync(Func<Email, bool> predicate)
        {
            return Task.FromResult(Emails.Where(predicate));
        }

        public Task<int> GetCountAsync(Expression<Func<Email, bool>> predicate)
        {
            return Task.FromResult(Emails.Count(predicate.Compile()));
        }

        public Task<Email> LastAsync()
        {
            return Task.FromResult(Emails.OrderBy(x => x.Info.Date).Last());
        }

        public Task<int> GetCountAsync()
        {
            return Task.FromResult(Emails.Count);
        }
    }
}