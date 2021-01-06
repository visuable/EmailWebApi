using System.Diagnostics.CodeAnalysis;

namespace EmailWebApi.Db.Entities
{
    public class EmailBody
    {
        /// <summary>
        ///     Тело сообщения.
        /// </summary>
        [NotNull]
        public virtual string Body { get; set; }

        /// <summary>
        ///     Флаг сохранения тела сообщения.
        /// </summary>
        [NotNull]
        public virtual bool Save { get; set; }

        public int Id { get; set; }
    }
}