namespace Fanda.Entities.Auth
{
    using System.Collections.Generic;

    public class Role : BaseTenantModel
    {
        public virtual ICollection<Privilege> Privileges { get; set; }
    }
}