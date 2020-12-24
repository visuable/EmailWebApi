using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Objects
{
    public class JsonResponse<T>
    {
        public T Output { get; set; }
    }
}
