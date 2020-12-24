using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailWebApi.Objects;

namespace EmailWebApi.Extensions
{
    public static class EmailExtensions
    {
        public static void SetState(this Email email, EmailStatus status)
        {
            email.State = new EmailState()
            {
                Status = status
            };
        }
    }
}
