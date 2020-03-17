using System;
using System.Collections.Generic;
using System.Text;

namespace Fanda.Data.Access
{
    public class UserRole
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
    }
}
