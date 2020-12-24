using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Objects
{
    public class EmailContent
    {
        public string Address { get; set; }
        public EmailBody Body { get; set; }
        public string Title { get; set; }
        public int Id { get; set; }
    }
}
