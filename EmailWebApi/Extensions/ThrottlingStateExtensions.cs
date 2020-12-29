using System;
using EmailWebApi.Entities;

namespace EmailWebApi.Extensions
{
    public static class ThrottlingStateExtensions
    {
        public static void UpdateAfterSending(this ThrottlingState state, string lastAddress)
        {
            state.Counter++;
            state.LastAddressCounter++;
            state.LastAddress = lastAddress;
        }

        public static void Refresh(this ThrottlingState state)
        {
            state.Counter = 0;
            state.LastAddressCounter = 0;
            state.EndPoint = DateTime.Now.AddSeconds(60);
        }
    }
}