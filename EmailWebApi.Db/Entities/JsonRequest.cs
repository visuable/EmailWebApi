namespace EmailWebApi.Db.Entities
{
    /// <summary>
    /// Универсальный шаблон запросов в JSON формате.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonRequest<T>
    {
        /// <summary>
        /// Входной параметр.
        /// </summary>
        public T Input { get; set; }
    }
}