//using Microsoft.AspNetCore.Identity;
using Fanda.Data.Business;
using System;
using System.Collections.Generic;

namespace Fanda.Data.Access
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
        //public string IPAddress { get; set; }
        //public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<OrgUserRole> OrgUserRoles { get; set; }

        public Role() : base()
        {
            DateCreated = DateTime.Now;
            Active = true;
        }

        public Role(string roleCode, string roleName, string description) //: base(roleName)
        {
            Code = roleCode;
            Name = roleName;
            Description = description;
            DateCreated = DateTime.Now;
            Active = true;
        }
    }
}