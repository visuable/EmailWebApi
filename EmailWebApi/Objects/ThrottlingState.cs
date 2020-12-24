using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Objects
{
    public class ThrottlingState
    {
        public int Counter { get; set; }
        public int LastAddressCounter { get; set; }
        public string LastAddress { get; set; }
        public DateTime EndPoint { get; set; }
        public int Id { get; set; }
    }
}
