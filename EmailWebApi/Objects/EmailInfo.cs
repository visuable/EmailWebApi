using System;

namespace EmailWebApi.Objects
{
    public class EmailInfo
    {
        public Guid UniversalId { get; set; }
        public DateTime Date { get; set; }
        public int Id { get; set; }
    }
}