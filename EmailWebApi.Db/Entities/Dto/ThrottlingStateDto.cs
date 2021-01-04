using System;

namespace EmailWebApi.Db.Entities.Dto
{
    public class ThrottlingStateDto
    {
        /// <summary>
        /// Общее число запросов.
        /// </summary>
        public int Counter { get; set; }
        /// <summary>
        /// Число запросов на последний адрес.
        /// </summary>
        public int LastAddressCounter { get; set; }
        /// <summary>
        /// Последний адрес отправки.
        /// </summary>
        public string LastAddress { get; set; }
    }
}