using System;
using System.Threading.Tasks;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Entities.Dto;
using EmailWebApi.Db.Repositories;
using EmailWebApi.Services.Interfaces;

namespace EmailWebApi.Services.Classes
{
    /// <summary>
    /// Предоставляет развернутую информацию о совершившихся запросах.
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
        /// Возвращает состояние.
        /// </summary>
        /// <returns>ThrottlingStateDto</returns>
        public async Task<ThrottlingStateDto> GetAsync()
        {
            var state = new ThrottlingStateDto();
            var offset = _dateTime.Now.AddMinutes(-1);
            try
            {
                var latestEmail = await _emailRepository.LastAsync();
                state.Counter = await _emailRepository.GetCountAsync(x => _dateTime.Now >= offset);
                state.LastAddress = latestEmail.Content.Address;
                state.LastAddressCounter = await _emailRepository.GetCountAsync(x =>
                    _dateTime.Now >= offset && x.Content.Address == latestEmail.Content.Address);
            }
            catch
            {
                state.Counter = 0;
                state.LastAddress = String.Empty;
                state.LastAddressCounter = 0;
            }

            return state;
        }
    }
}