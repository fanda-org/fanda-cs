//using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Fanda.Models
{
    public class User : RootModel
    {
        public User()
        {
            OrgUsers = new HashSet<OrgUser>();
        }
        //public string UserName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateLastLogin { get; set; }
        public virtual ICollection<OrgUser> OrgUsers { get; set; }
    }
}