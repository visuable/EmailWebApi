using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EmailWebApi.Db.Repositories
{
    /// <summary>
    ///     Репозиторий.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        ///     Добавить в базу данных.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task InsertAsync(TEntity entity);

        /// <summary>
        ///     Обновляет в базе данных.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        ///     Первый элемент из списка по предикату.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Email</returns>
        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///     Все элементы по предикату.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllAsync(Func<TEntity, bool> predicate);

        /// <summary>
        ///     Количество элементов по предикату.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>int</returns>
        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///     Первый элемент из списка.
        /// </summary>
        /// <returns>Email</returns>
        Task<TEntity> FirstAsync();

        /// <summary>
        ///     Последний элемент.
        /// </summary>
        /// <returns></returns>
        Task<TEntity> LastAsync();

        /// <summary>
        ///     Количество всех элементов.
        /// </summary>
        /// <returns>int</returns>
        Task<int> GetCountAsync();
    }
}