using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Models.Dto
{
    public class EmailStatusDto
    {
        public bool IsArrived { get; set; }
        public string SentDate { get; set; }
        public string SentTime { get; set; }
        public Guid EmailId { get; set; }
    }
}
