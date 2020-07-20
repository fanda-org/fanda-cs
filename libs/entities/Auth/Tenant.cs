namespace Fanda.Entities.Auth
{
    using System.Collections.Generic;

    public class Tenant : BaseEntity
    {
        public int OrgCount { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}