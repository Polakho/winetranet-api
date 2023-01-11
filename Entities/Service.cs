using System;
using System.Collections.Generic;

#nullable disable

namespace winetranet_api.Entities
{
    public partial class Service
    {
        public Service()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
