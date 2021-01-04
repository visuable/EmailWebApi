namespace EmailWebApi.Db.Entities
{
    /// <summary>
    /// Статус сообщения.
    /// </summary>
    public enum EmailStatus
    {
        /// <summary>
        /// Ошибка.
        /// </summary>
        Error,
        /// <summary>
        /// Отправлено
        /// </summary>
        Sent,
        /// <summary>
        /// В очереди.
        /// </summary>
        Query
    }
}