using System;

namespace Fanda.Data
{
    public class OrgContact
    {
        public Guid OrgId { get; set; }
        public Guid ContactId { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual Contact Contact { get; set; }
    }
}