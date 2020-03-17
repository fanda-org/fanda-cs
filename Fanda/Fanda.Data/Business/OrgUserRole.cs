using Fanda.Data.Access;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fanda.Data.Business
{
    public class OrgUserRole
    {
        public Guid OrgId { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        //public Organization Organization { get; set; }
        //public User User { get; set; }
        public OrgUser OrgUser { get;set; }
        public Role Role { get; set; }
    }
}
