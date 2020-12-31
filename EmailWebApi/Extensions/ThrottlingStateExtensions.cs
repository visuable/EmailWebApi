using System;
using EmailWebApi.Db.Entities.Dto;

namespace EmailWebApi.Extensions
{
    public static class ThrottlingStateExtensions
    {
        public static void UpdateAfterSending(this ThrottlingStateDto stateDto, string lastAddress)
        {
            stateDto.Counter++;
            stateDto.LastAddressCounter++;
            stateDto.LastAddress = lastAddress;
        }

        public static void Refresh(this ThrottlingStateDto stateDto)
        {
            stateDto.Counter = 0;
            stateDto.LastAddressCounter = 0;
            stateDto.EndPoint = DateTime.Now.AddSeconds(60);
        }
    }
}