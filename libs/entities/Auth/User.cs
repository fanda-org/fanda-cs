//using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Fanda.Entities.Auth
{
    public class User : EmailEntity
    {
        [JsonIgnore]
        public string PasswordHash { get; set; }
        [JsonIgnore]
        public string PasswordSalt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateLastLogin { get; set; }

        [JsonIgnore]
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}