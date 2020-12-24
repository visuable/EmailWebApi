using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Objects
{
    public class JsonRequest<T>
    {
        public T Input { get; set; }
    }
}
