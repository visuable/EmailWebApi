using System;
using System.Diagnostics.CodeAnalysis;

namespace EmailWebApi.Db.Entities
{
    public class EmailInfo
    {
        /// <summary>
        ///     Универсальный идентификатор сообщения (GUID).
        /// </summary>
        [NotNull]
        public virtual Guid UniversalId { get; set; }

        /// <summary>
        ///     Дата отправки сообщения.
        /// </summary>

        [NotNull]
        public virtual DateTime Date { get; set; }

        public int Id { get; set; }
    }
}