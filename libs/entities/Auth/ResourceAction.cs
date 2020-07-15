namespace Fanda.Entities.Auth
{
    using System;
    using System.Collections.Generic;

    public class ResourceAction
    {
        public Guid Id { get; set; }

        public Guid ResourceId { get; set; }
        public Guid ActionId { get; set; }

        public virtual Resource Resource { get; set; }
        public virtual Action Action { get; set; }
        public virtual ICollection<Privilege> Privileges { get; set; }
    }
}