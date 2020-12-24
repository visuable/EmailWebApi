using EmailWebApi.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Services
{
    public interface IEmailTransferService
    {
        Email Send(Email email);
    }
}
