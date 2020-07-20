namespace Fanda.Entities.Auth
{
    using System.Collections.Generic;

    public class Role : BaseTenantEntity
    {
        public ICollection<Privilege> Privileges { get; set; }
    }
}