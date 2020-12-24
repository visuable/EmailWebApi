using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Objects
{
    public class EmailBody
    {
        public string Body { get; set; }
        public bool Save { get; set; }
        public int Id { get; set; }
    }
}
