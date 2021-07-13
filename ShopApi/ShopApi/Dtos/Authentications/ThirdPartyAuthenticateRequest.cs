using ShopApi.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Dtos.Authentications
{
    public class ThirdPartyAuthenticateRequest
    {
        public string KeyProvided { get; set; }

        public ProviderType ProviderType { get; set; }
    }
}
