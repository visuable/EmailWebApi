using System;
using System.Diagnostics.CodeAnalysis;

namespace EmailWebApi.Objects
{
    public class ThrottlingState
    {
        [NotNull]
        public virtual int Counter { get; set; }
        [NotNull]
        public virtual int LastAddressCounter { get; set; }
        [NotNull]
        public virtual string LastAddress { get; set; }
        [NotNull]
        public virtual DateTime EndPoint { get; set; }
        public int Id { get; set; }
    }
}