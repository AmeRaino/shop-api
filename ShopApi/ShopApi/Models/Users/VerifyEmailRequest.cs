﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Models.Users
{
    public class VerifyEmailRequest
    {
        public string Token { get; set; }
    }
}
