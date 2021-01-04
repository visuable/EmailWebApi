using System.Threading.Tasks;
using EmailWebApi.Db.Entities.Dto;

namespace EmailWebApi.Services.Interfaces
{
    /// <summary>
    /// Предоставляет развернутую информацию о совершившихся запросах.
    /// </summary>
    public interface IThrottlingStateProviderService
    {
        /// <summary>
        /// Возвращает состояние.
        /// </summary>
        /// <returns>ThrottlingStateDto</returns>
        Task<ThrottlingStateDto> GetAsync();
    }
}