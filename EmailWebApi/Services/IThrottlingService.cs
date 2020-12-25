using System.Collections.Generic;
using System.Threading.Tasks;
using EmailWebApi.Objects;

namespace EmailWebApi.Services
{
    public interface IThrottlingService
    {
        Task Invoke(Email email);
    }
}