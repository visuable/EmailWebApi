using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailWebApi.Objects;

namespace EmailWebApi.Extensions
{
    public static class ThrottlingStateExtensions
    {
        public static void RefreshCounter(this ThrottlingState state)
        {
            state.Counter = 0;
        }

        public static void RefreshLastAddressCounter(this ThrottlingState state)
        {
            state.LastAddressCounter = 0;
        }

        public static void RefreshEndPoint(this ThrottlingState state)
        {
            state.EndPoint = DateTime.Now.AddSeconds(60);
        }

        public static void RefreshLastAddress(this ThrottlingState state, string lastAddress)
        {
            state.LastAddress = lastAddress;
        }

        public static void IncrementCounter(this ThrottlingState state)
        {
            state.Counter++;
        }

        public static void IncrementLastAddressCounter(this ThrottlingState state)
        {
            state.LastAddressCounter++;
        }
    }
}
