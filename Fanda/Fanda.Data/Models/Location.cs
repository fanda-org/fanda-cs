using System;
using System.Collections.Generic;

namespace Fanda.Data.Models
{
    public class Location
    {
        public Guid LocationId { get; set; }
        public Guid OrgId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}