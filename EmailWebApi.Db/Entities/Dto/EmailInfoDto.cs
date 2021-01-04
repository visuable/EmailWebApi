using System;

namespace EmailWebApi.Db.Entities.Dto
{
    public class EmailInfoDto
    {
        /// <summary>
        /// Уникальный идентификатор (GUID) сообщения в базе данных.
        /// </summary>
        public Guid UniversalId { get; set; }
        /// <summary>
        /// Время отправки письма.
        /// </summary>
        public DateTime Date { get; set; }
    }
}