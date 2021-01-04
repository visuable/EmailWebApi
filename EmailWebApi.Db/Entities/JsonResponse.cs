namespace EmailWebApi.Db.Entities
{
    /// <summary>
    /// Универсальный шаблон ответов в JSON формате.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonResponse<T>
    {
        /// <summary>
        /// Выходной параметр.
        /// </summary>
        public T Output { get; set; }
    }
}