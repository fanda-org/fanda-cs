using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fanda.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }

        [StringLength(16)]
        public string UserName { get; set; }

        [StringLength(255)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public string Token { get; set; }

        //public bool EmailConfirmed { get; set; }
        //public bool PhoneNumberConfirmed { get; set; }
        //public int AccessFailedCount { get; set; }
        public bool Active { get; set; }
        public DateTime? DateLastLogin { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public virtual ICollection<OrganizationDto> Organizations { get; set; }
    }
}