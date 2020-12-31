using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EmailWebApi.Db.Database;
using EmailWebApi.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmailWebApi.Db.Repositories
{
    public class DbContextEmailRepository : IEmailRepository
    {
        private readonly EmailContext _context;

        public DbContextEmailRepository(EmailContext context)
        {
            _context = context;
        }

        public async Task<int> CountAsync(Expression<Func<Email, bool>> predicate)
        {
            return await _context.Emails.CountAsync(predicate);
        }

        public async Task AddAsync(Email item)
        {
            await _context.Emails.AddAsync(item);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Update(Email item)
        {
            _context.Update(item);
        }

        public async Task<int> CountAsync()
        {
            return await _context.Emails.CountAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Email> GetFirstAsync()
        {
            return await _context.Emails.FirstOrDefaultAsync();
        }

        public IEnumerable<Email> GetListByPredicate(Expression<Func<Email, bool>> predicate)
        {
            return _context.Emails.Where(predicate.Compile());
        }

        public async Task<IEnumerable<Email>> GetListByPredicateAsync(Expression<Func<Email, bool>> predicate)
        {
            return (await _context.Emails.ToListAsync()).Where(predicate.Compile());
        }
    }
}