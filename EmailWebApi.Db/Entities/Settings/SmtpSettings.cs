namespace EmailWebApi.Db.Entities.Settings
{
    public class SmtpSettings
    {
        /// <summary>
        ///     Хостинг.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        ///     Порт.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        ///     Имя пользователя.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     Пароль.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     Почта отправителя.
        /// </summary>
        public string SenderEmail { get; set; }
    }
}