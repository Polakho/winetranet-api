using System;
using System.Collections.Generic;

#nullable disable

namespace winetranet_api.DTO
{
    public partial class UserDTO
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PhoneMobile { get; set; }
        public string Role { get; set; }
        public int? Service { get; set; }
        public int? Site { get; set; }

    }
}
