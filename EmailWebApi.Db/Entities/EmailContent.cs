using System.Diagnostics.CodeAnalysis;

namespace EmailWebApi.Db.Entities
{
    public class EmailContent
    {
        /// <summary>
        ///     Адрес отправителя.
        /// </summary>
        [NotNull]
        public virtual string Address { get; set; }

        /// <summary>
        ///     Тело сообщения.
        /// </summary>

        [NotNull]
        public virtual EmailBody Body { get; set; }

        /// <summary>
        ///     Заголовок сообщения.
        /// </summary>

        [NotNull]
        public virtual string Title { get; set; }

        public int Id { get; set; }
    }
}