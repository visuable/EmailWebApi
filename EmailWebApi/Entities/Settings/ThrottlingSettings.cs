﻿namespace EmailWebApi.Entities.Settings
{
    public class ThrottlingSettings
    {
        public int Limit { get; set; }
        public int AddressLimit { get; set; }
    }
}