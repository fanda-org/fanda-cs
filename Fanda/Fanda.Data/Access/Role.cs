using Microsoft.AspNetCore.Identity;
using System;

namespace Fanda.Data.Access
{
    public class Role : IdentityRole<Guid>
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool Active { get; set; }
        //public string IPAddress { get; set; }

        public Role() : base()
        {
            DateCreated = DateTime.Now;
            Active = true;
        }

        public Role(string roleCode, string roleName, string description) : base(roleName)
        {
            Code = roleCode;
            Description = description;
            DateCreated = DateTime.Now;
            Active = true;
        }
    }
}