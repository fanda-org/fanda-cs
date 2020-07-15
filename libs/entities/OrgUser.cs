using System;
using System.Collections.Generic;

namespace Fanda.Entities
{
    public class OrgUser
    {
        public Guid OrgId { get; set; }
        public Guid UserId { get; set; }

        public virtual Organization Organization { get; set; }
        //public virtual User User { get; set; }
        //public virtual ICollection<OrgUserRole> OrgUserRoles { get; set; }
    }
}