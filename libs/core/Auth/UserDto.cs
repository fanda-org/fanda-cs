namespace Fanda.Core.Auth
{
    using Fanda.Core.Base;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UserDto : RootDto
    {
        [Required]
        [Display(Name = "Tenant ID")]
        public Guid TenantId { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }

        public DateTime? DateLastLogin { get; set; }
    }

    public class UserListDto : RootListDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}