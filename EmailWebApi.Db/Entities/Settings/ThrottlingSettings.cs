namespace EmailWebApi.Db.Entities.Settings
{
    public class ThrottlingSettings
    {
        /// <summary>
        ///     Общий лимит на отправку.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        ///     Лимит отправки на один и тот же адрес.
        /// </summary>
        public int AddressLimit { get; set; }
    }
}