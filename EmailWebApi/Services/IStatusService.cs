using EmailWebApi.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Services
{
    public interface IStatusService
    {
        EmailState GetEmailState(EmailInfo info);
        ApplicationState GetApplicationState();
    }
}
