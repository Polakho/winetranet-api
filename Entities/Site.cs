using System;
using System.Collections.Generic;

#nullable disable

namespace winetranet_api.Entities
{
    public partial class Site
    {
        public Site()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Ville { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
