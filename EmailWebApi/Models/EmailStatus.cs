using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Models
{
    public class EmailStatus
    {
        [Key]
        public int Id { get; set; }
        public bool IsArrived { get; set; }
        public string SentDate { get; set; }
        public string SentTime { get; set; }
        public Guid EmailId { get; set; }
    }
}
