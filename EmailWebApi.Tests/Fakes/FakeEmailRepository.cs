using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Repositories;

namespace EmailWebApi.Tests.Fakes
{
    public class FakeEmailRepository : IEmailRepository
    {
        public FakeEmailRepository()
        {
            Emails = new List<Email>();
        }

        public List<Email> Emails { get; set; }

        public Task<int> CountAsync(Expression<Func<Email, bool>> predicate)
        {
            return Task.FromResult(Emails.Count(predicate.Compile()));
        }

        public Task AddAsync(Email item)
        {
            Emails.Add(item);
            return Task.CompletedTask;
        }

        public void SaveChanges()
        {
        }

        public void Update(Email item)
        {
            var last = Emails.FirstOrDefault(x => x.Content.Body.Body == item.Content.Body.Body);
            Emails[last.Id] = item;
        }

        public Task<int> CountAsync()
        {
            return Task.FromResult(Emails.Count);
        }

        public Task SaveChangesAsync()
        {
            return Task.CompletedTask;
        }

        public Task<Email> GetFirstAsync()
        {
            return Task.FromResult(Emails.FirstOrDefault());
        }

        public IEnumerable<Email> GetListByPredicate(Expression<Func<Email, bool>> predicate)
        {
            return Emails.Where(predicate.Compile());
        }

        public Task<IEnumerable<Email>> GetListByPredicateAsync(Expression<Func<Email, bool>> predicate)
        {
            return Task.FromResult(Emails.Where(predicate.Compile()));
        }
    }
}