using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Dtos.Authentications
{
    public class ThirdPartyAuthenticateResponse
    {
        public string Id { get; set; }
        public string Token { get; set; }
    }
}
