using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Models
{
    public class Email
    {
        public string Adress { get; set; }
        public string Body { get; set; }
        public string Title { get; set; }
        public EmailStatus EmailStatus { get; set; }
        [Key]
        public int Id { get; set; }
    }
}
