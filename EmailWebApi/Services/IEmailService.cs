using EmailWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Services
{
    public interface IEmailService
    {
        Task<Guid> SendEmail(Email email);
        Email GetEmailStatus(Guid id);
        StatusModel GetCurrentStatus();
    }
}
