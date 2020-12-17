using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Settings
{
    public class ThrottlerSettings
    {
        public int GlobalRequestPerMinute { get; set; }
    }
}
