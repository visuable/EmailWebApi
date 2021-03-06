﻿namespace EmailWebApi.Db.Entities
{
    public enum EmailStatus
    {
        /// <summary>
        ///     Ошибка.
        /// </summary>
        Error,

        /// <summary>
        ///     Отправлено
        /// </summary>
        Sent,

        /// <summary>
        ///     В очереди.
        /// </summary>
        Query
    }
}