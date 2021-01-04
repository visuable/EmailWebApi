namespace EmailWebApi.Db.Entities.Dto
{
    public class EmailContentDto
    {
        /// <summary>
        /// Адрес получателя.
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Тело письма.
        /// </summary>
        public EmailBodyDto Body { get; set; }
        /// <summary>
        /// Заголовок письма.
        /// </summary>
        public string Title { get; set; }
    }
}