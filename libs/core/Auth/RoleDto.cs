namespace Fanda.Core.Auth
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Fanda.Core.Base;

    public class RoleDto : BaseDto
    {
        [Required]
        public Guid TenantId { get; set; }
        public ICollection<PrivilegeDto> Privileges { get; set; }
    }

    public class RoleListDto : BaseListDto
    {

    }

    public class RoleChildrenDto
    {
        public ICollection<PrivilegeDto> Privileges { get; set; }
    }
}