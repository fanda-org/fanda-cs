using System;

namespace Fanda.Models
{
    public class OrgUserRole
    {
        public Guid OrgId { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        public virtual OrgUser OrgUser { get; set; }
        public virtual Role Role { get; set; }
    }
}
