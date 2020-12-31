using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EmailWebApi.Db.Entities;

namespace EmailWebApi.Db.Repositories
{
    public interface IEmailRepository
    {
        Task<int> CountAsync(Expression<Func<Email, bool>> predicate);
        Task AddAsync(Email item);
        void SaveChanges();
        void Update(Email item);
        Task<int> CountAsync();
        Task SaveChangesAsync();
        Task<Email> GetFirstAsync();
        IEnumerable<Email> GetListByPredicate(Expression<Func<Email, bool>> predicate);
        Task<IEnumerable<Email>> GetListByPredicateAsync(Expression<Func<Email, bool>> predicate);
    }
}