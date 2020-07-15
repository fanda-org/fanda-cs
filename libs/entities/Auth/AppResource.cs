namespace Fanda.Entities.Auth
{
    using System;
    using System.Collections.Generic;

    public class AppResource
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid ResourceId { get; set; }

        public virtual Application Application { get; set; }
        public virtual Resource Resource { get; set; }

        public virtual ICollection<Privilege> Privileges { get; set; }
    }
}