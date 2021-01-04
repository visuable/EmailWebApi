using System;
using EmailWebApi.Db.Entities;

namespace EmailWebApi.Extensions
{
    /// <summary>
    /// Методы-расширения Email.
    /// </summary>
    public static class EmailExtensions
    {
        /// <summary>
        /// Устанавливает EmailStatus.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="status"></param>
        public static void SetState(this Email email, EmailStatus status)
        {
            if (email.State != null)
                email.State.Status = status;
            else
                email.State = new EmailState
                {
                    Status = status
                };
        }
        /// <summary>
        /// Устанавливает EmailInfo.
        /// </summary>
        /// <param name="email"></param>
        public static void SetEmailInfo(this Email email)
        {
            if (email.Info != null)
                email.Info.Date = DateTime.Now;
            else
                email.Info = new EmailInfo
                {
                    Date = DateTime.Now,
                    UniversalId = Guid.NewGuid()
                };
        }
    }
}