//using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
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
        [JsonIgnore]
        public byte[] PasswordHash { get; set; }
        [JsonIgnore]
        public byte[] PasswordSalt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateLastLogin { get; set; }
        [JsonIgnore]
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        public virtual ICollection<OrgUser> OrgUsers { get; set; }
    }
}