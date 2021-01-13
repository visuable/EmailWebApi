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
        /// <summary>
        /// Имя отправителя.
        /// </summary>
        public string SenderName { get; set; }
        /// <summary>
        /// Тип текста в сообщении.
        /// </summary>
        /// <remarks>Обычный текст: Plain. Для HTML: Html</remarks>
        public string TextType { get; set; }
        /// <summary>
        /// Имя файла для записи лога SmtpClient.
        /// </summary>
        public string LogFileName { get; set; }
    }
}