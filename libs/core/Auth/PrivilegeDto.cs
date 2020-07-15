namespace Fanda.Core.Auth
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PrivilegeDto
    {
        // Role
        [Required]
        public Guid RoleId { get; set; }
        // AppResourceAction
        [Required]
        public Guid ApplicationId { get; set; }
        [Required]
        public Guid ResourceId { get; set; }
        [Required]
        public Guid ActionId { get; set; }
    }
}