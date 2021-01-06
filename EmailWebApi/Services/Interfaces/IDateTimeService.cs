using System;

namespace EmailWebApi.Services.Interfaces
{
    /// <summary>
    ///     Предоставляет время и дату.
    /// </summary>
    public interface IDateTimeService
    {
        /// <summary>
        ///     Сейчас.
        /// </summary>
        DateTimeOffset Now { get; set; }
    }
}