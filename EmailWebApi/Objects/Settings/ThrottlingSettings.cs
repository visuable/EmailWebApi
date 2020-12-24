using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Objects.Settings
{
    public class ThrottlingSettings
    {
        public int Limit { get; set; }
        public int AddressLimit { get; set; }
    }
}
