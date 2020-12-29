using System;
using System.Diagnostics.CodeAnalysis;

namespace EmailWebApi.Entities
{
    public class EmailInfo
    {
        [NotNull] public virtual Guid UniversalId { get; set; }

        [NotNull] public virtual DateTime Date { get; set; }

        public int Id { get; set; }
    }
}