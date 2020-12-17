using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Services
{
    public interface IThrottlerService<TResult>
    {
        Task<Guid> Invoke(Task<TResult> func);
    }
}
