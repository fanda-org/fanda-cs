namespace Fanda.Entities.Auth
{
    using System;

    public class EmailEntity : RootEntity
    {
        public Guid TenantId { get; set; }
        public string Email { get; set; }
        public virtual Tenant Tenant { get; set; }
    }

    // public class BaseAppModel : BaseParentModel
    // {
    //     public Guid ApplicationId { get; set; }
    //     public virtual Application Application { get; set; }
    // }

    public class BaseTenantEntity : BaseEntity
    {
        public Guid TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }
    }
}