using Fanda.Data.Base;
using System;

namespace Fanda.Data.Business
{
    public class OrgContact
    {
        public Guid OrgId { get; set; }
        public Guid ContactId { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual Contact Contact { get; set; }
    }
}