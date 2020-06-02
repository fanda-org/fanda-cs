using Fanda.Dto.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fanda.Dto
{
    public class UserDto : RootDto
    {
        //[StringLength(16)]
        //public string UserName { get; set; }

        [StringLength(255)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }

        //public virtual ICollection<RefreshTokenDto> RefreshTokens { get; set; }

        //public bool EmailConfirmed { get; set; }
        //public bool PhoneNumberConfirmed { get; set; }
        //public int AccessFailedCount { get; set; }
        //public virtual ICollection<OrganizationDto> Organizations { get; set; }
    }

    public class UserListDto : RootListDto
    {
        public string Email { get; set; }
        //public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public Guid OrgId { get; set; }
    }
}