using System;
using System.ComponentModel.DataAnnotations;

namespace Fanda.Dto
{
    public class UserDto
    {
        public string UserId { get; set; }

        [StringLength(16)]
        public string UserName { get; set; }

        [StringLength(255)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        //[DataType(DataType.Password)]
        //public string Password { get; set; }

        //public string PhoneNumber { get; set; }

        public string Token { get; set; }

        //public bool EmailConfirmed { get; set; }
        //public bool PhoneNumberConfirmed { get; set; }
        //public int AccessFailedCount { get; set; }
        public string LocationId { get; set; }
        public bool Active { get; set; }
        public DateTime? DateLastLogin { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}