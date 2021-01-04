using System;
using EmailWebApi.Services.Interfaces;

namespace EmailWebApi.Services.Classes
{
    /// <summary>
    /// Предоставляет системное время и дату.
    /// </summary>
    public class SystemDateTimeService : IDateTimeService
    {
        public SystemDateTimeService()
        {
            Now = DateTimeOffset.Now;
        }
        /// <summary>
        /// Сейчас.
        /// </summary>
        public DateTimeOffset Now { get; set; }
    }
}