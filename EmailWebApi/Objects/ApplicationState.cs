using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Objects
{
    public class ApplicationState
    {
        public int Total { get; set; }
        public int Error { get; set; }
        public int Sent { get; set; }
        public int Query { get; set; }
    }
}
