using EmailWebApi.Db.Entities.Dto;

namespace EmailWebApi.Extensions
{
    /// <summary>
    ///     Методы-расширения ThrottlingStateDto.
    /// </summary>
    public static class ThrottlingStateExtensions
    {
        /// <summary>
        ///     Возвращает состояние с нулевыми значениями.
        /// </summary>
        /// <remarks>Необходимо, если база данных не содержит значений.</remarks>
        /// <param name="throttlingState"></param>
        public static void Empty(this ThrottlingStateDto throttlingState)
        {
            throttlingState.Counter = 0;
            throttlingState.LastAddress = string.Empty;
            throttlingState.LastAddressCounter = 0;
        }
    }
}