namespace EmailWebApi.Db.Entities.Dto
{
    public class EmailBodyDto
    {
        /// <summary>
        /// Тело сообщения.
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// Флаг о сохранении тела сообщения.
        /// </summary>
        public bool Save { get; set; }
    }
}