using System.Diagnostics.CodeAnalysis;

namespace EmailWebApi.Entities
{
    public class Email
    {
        [NotNull] public virtual EmailContent Content { get; set; }

        [NotNull] public virtual EmailInfo Info { get; set; }

        [NotNull] public virtual EmailState State { get; set; }

        public int Id { get; set; }
    }
}