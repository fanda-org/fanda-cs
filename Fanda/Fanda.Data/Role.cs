//using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Fanda.Data
{
    public class Role //: IdentityRole<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<OrgUserRole> OrgUserRoles { get; set; }
    }
}