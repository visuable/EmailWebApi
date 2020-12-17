using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Models.Controllers
{
    public class JsonRequest<T>
    {
        public T Value { get; set; }
        public JsonRequest(T value)
        {
            Value = value;
        }
        public JsonRequest()
        {

        }
    }
}
