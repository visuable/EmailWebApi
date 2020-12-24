using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Objects
{
    public class Email
    {
        public EmailContent Content { get; set; }
        public EmailInfo Info { get; set; }
        public EmailState State { get; set; }
        public int Id { get; set; }
    }
}
