using System.Diagnostics.CodeAnalysis;

namespace EmailWebApi.Db.Entities
{
    public class EmailBody
    {
        [NotNull] public virtual string Body { get; set; }

        [NotNull] public virtual bool Save { get; set; }

        public int Id { get; set; }
    }
}