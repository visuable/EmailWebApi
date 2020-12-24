using System.Collections.Generic;
using EmailWebApi.Objects;

namespace EmailWebApi.Services
{
    public interface IThrottlingService
    {
        List<Email> Invoke(Email email);
    }
}