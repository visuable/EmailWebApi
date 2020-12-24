using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Objects
{
    public class EmailState
    {
        public EmailStatus Status { get; set; }
        public int Id { get; set; }
    }
}
