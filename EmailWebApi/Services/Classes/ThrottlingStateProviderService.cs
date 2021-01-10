using System.Threading.Tasks;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Entities.Dto;
using EmailWebApi.Db.Repositories;
using EmailWebApi.Extensions;
using EmailWebApi.Services.Interfaces;

namespace EmailWebApi.Services.Classes
{
    /// <summary>
    ///     Предоставляет развернутую информацию о совершившихся запросах.
    /// </summary>
    public class ThrottlingStateProviderService : IThrottlingStateProviderService
    {
        private readonly IDateTimeService _dateTime;
        private readonly IRepository<Email> _emailRepository;

        public ThrottlingStateProviderService(IRepository<Email> emailRepository, IDateTimeService dateTime)
        {
            _emailRepository = emailRepository;
            _dateTime = dateTime;
        }

        /// <summary>
        ///     Возвращает состояние.
        /// </summary>
        /// <remarks>Возвращает пустое состояние, если база данных пуста.</remarks>
        /// <returns>ThrottlingStateDto</returns>
        public async Task<ThrottlingStateDto> GetAsync()
        {
            var state = new ThrottlingStateDto();
            var offset = _dateTime.Now.AddMinutes(-1);
            try
            {
                var latestEmail = await _emailRepository.LastAsync();
                state.Counter = await _emailRepository.GetCountAsync(x => x.Info.Date >= offset.DateTime);
                state.LastAddress = latestEmail.Content.Address;
                state.LastAddressCounter = await _emailRepository.GetCountAsync(x =>
                    x.Info.Date >= offset.DateTime && x.Content.Address == latestEmail.Content.Address);
            }
            catch
            {
                state.Clear();
            }

            return state;
        }
    }
}