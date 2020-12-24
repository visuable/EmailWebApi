using EmailWebApi.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Services
{
    public interface IThrottlingService
    {
        List<Email> Invoke(Email email);
    }
}
