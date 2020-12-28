using System;
using EmailWebApi.Objects;

namespace EmailWebApi.Extensions
{
    public static class EmailExtensions
    {
        public static void SetState(this Email email, EmailStatus status)
        {
            if (email.State != null)
            {
                email.State.Status = status;
                return;
            }
            email.State = new EmailState
            {
                Status = status
            };
        }

        public static void SetEmailInfo(this Email email)
        {
            if (email.Info != null)
            {
                email.Info.Date = DateTime.Now;
            }
            else
            {
                email.Info = new EmailInfo
                {
                    Date = DateTime.Now,
                    UniversalId = Guid.NewGuid()
                };
            }
        }
    }
}