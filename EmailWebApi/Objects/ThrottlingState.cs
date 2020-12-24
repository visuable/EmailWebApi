using System;

namespace EmailWebApi.Objects
{
    public class ThrottlingState
    {
        public int Counter { get; set; }
        public int LastAddressCounter { get; set; }
        public string LastAddress { get; set; }
        public DateTime EndPoint { get; set; }
        public int Id { get; set; }
    }
}