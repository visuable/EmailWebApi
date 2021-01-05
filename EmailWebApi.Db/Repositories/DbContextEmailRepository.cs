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
    /// <summary>
    /// Репозиторий сообщений в базе данных.
    /// </summary>
    public class DbContextEmailRepository : IRepository<Email>
    {
        private readonly EmailContext _emailContext;

        public DbContextEmailRepository(EmailContext emailContext)
        {
            _emailContext = emailContext;
        }
        /// <summary>
        /// Первое сообщение из списка по предикату.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Email</returns>
        public async Task<Email> FirstAsync(Expression<Func<Email, bool>> predicate)
        {
            return await _emailContext.Emails.FirstAsync(predicate);
        }
        /// <summary>
        /// Первое сообщение из списка.
        /// </summary>
        /// <returns>Email</returns>
        public async Task<Email> FirstAsync()
        {
            return await _emailContext.Emails.FirstAsync();
        }
        /// <summary>
        /// Все сообщения по предикату.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Email>> GetAllAsync(Func<Email, bool> predicate)
        {
            return (await _emailContext.Emails.ToListAsync()).Where(predicate);
        }
        /// <summary>
        /// Последнее сообщение.
        /// </summary>
        /// <returns></returns>
        public async Task<Email> LastAsync()
        {
            return await _emailContext.Emails.OrderBy(x => x.Info.Date).LastAsync();
        }
        /// <summary>
        /// Количество сообщений по предикату.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>int</returns>
        public async Task<int> GetCountAsync(Expression<Func<Email, bool>> predicate)
        {
            return await _emailContext.Emails.CountAsync(predicate);
        }
        /// <summary>
        /// Количество всех сообщений.
        /// </summary>
        /// <returns>int</returns>
        public async Task<int> GetCountAsync()
        {
            return await _emailContext.Emails.CountAsync();
        }
        /// <summary>
        /// Добавить сообщение в базу данных.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task InsertAsync(Email entity)
        {
            await _emailContext.Emails.AddAsync(entity);
            await _emailContext.SaveChangesAsync();
        }
        /// <summary>
        /// Обновить сообщение в базе данных.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Email entity)
        {
            _emailContext.Emails.Update(entity);
            await _emailContext.SaveChangesAsync();
        }
    }
}