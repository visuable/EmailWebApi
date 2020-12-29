using System;
using System.Diagnostics.CodeAnalysis;

namespace EmailWebApi.Entities.Dto
{
    public class ThrottlingStateDto
    {
        public int Counter { get; set; }

        public int LastAddressCounter { get; set; }

        public string LastAddress { get; set; }

        public DateTime EndPoint { get; set; }
    }
}