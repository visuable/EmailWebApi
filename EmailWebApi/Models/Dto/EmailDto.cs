using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Models.Dto
{
    public class EmailDto
    {
        public string Adress { get; set; }
        public string Body { get; set; }
        public string Title { get; set; }
        public EmailStatusDto EmailStatus { get; set; }
    }
}
