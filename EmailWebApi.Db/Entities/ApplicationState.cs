namespace EmailWebApi.Db.Entities
{
    public class ApplicationState
    {
        /// <summary>
        ///     Всего.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        ///     Ошибки.
        /// </summary>
        public int Error { get; set; }

        /// <summary>
        ///     Отправлено.
        /// </summary>
        public int Sent { get; set; }

        /// <summary>
        ///     В очереди.
        /// </summary>
        public int Query { get; set; }
    }
}