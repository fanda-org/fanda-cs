namespace Fanda.Core.Auth
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Fanda.Core.Base;

    public class RoleDto : BaseDto
    {
        [Required]
        public Guid TenantId { get; set; }
    }

    public class RoleListDto : BaseListDto
    {

    }
}