﻿using System.Diagnostics.CodeAnalysis;

namespace EmailWebApi.Objects
{
    public class EmailState
    {
        [NotNull]
        public virtual EmailStatus Status { get; set; }
        public int Id { get; set; }
    }
}