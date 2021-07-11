using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Dtos.Users
{
    public class UserResponseModel
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public long? Birthday { get; set; }
        public string? Phone { get; set; }
        public string? ImageUrl { get; set; }
    }
}
