namespace EmailWebApi.Db.Entities.Dto
{
    public enum EmailStatusDto
    {
        /// <summary>
        ///     Отправлено
        /// </summary>
        Sent,

        /// <summary>
        ///     В очереди.
        /// </summary>
        Query,

        /// <summary>
        ///     Ошибка.
        /// </summary>
        Error
    }
}