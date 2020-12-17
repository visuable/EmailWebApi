﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Models.Dto
{
    public class StatusModelDto
    {
        public int Total { get; set; }
        public int Sent { get; set; }
        public int Failed { get; set; }
        public int Query { get; set; }
    }
}