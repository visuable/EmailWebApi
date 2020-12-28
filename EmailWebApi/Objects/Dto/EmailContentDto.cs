using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Objects.Dto
{
    public class EmailContentDto
    {
        public string Address { get; set; }
        public EmailBodyDto Body { get; set; }
        public string Title { get; set; }
    }
}
