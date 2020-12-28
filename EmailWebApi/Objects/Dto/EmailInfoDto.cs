using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Objects.Dto
{
    public class EmailInfoDto
    {
        public Guid UniversalId { get; set; }
        public DateTime Date { get; set; }
    }
}
