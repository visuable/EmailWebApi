using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Models.Controllers
{
    public class JsonResponse<T>
    {
        public T Value { get; set; }
        public JsonResponse(T value)
        {
            Value = value;
        }
        public JsonResponse()
        {

        }
    }
}
