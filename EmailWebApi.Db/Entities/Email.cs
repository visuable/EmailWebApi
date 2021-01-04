using System.Diagnostics.CodeAnalysis;

namespace EmailWebApi.Db.Entities
{
    public class Email
    {
        /// <summary>
        /// Содержание сообщения.
        /// </summary>
        [NotNull] public virtual EmailContent Content { get; set; }
        /// <summary>
        /// Информация сообщения.
        /// </summary>

        [NotNull] public virtual EmailInfo Info { get; set; }
        /// <summary>
        /// Состояние сообщения.
        /// </summary>

        [NotNull] public virtual EmailState State { get; set; }

        public int Id { get; set; }
    }
}