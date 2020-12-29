using System.Diagnostics.CodeAnalysis;

namespace EmailWebApi.Entities
{
    public class EmailContent
    {
        [NotNull] public virtual string Address { get; set; }

        [NotNull] public virtual EmailBody Body { get; set; }

        [NotNull] public virtual string Title { get; set; }

        public int Id { get; set; }
    }
}