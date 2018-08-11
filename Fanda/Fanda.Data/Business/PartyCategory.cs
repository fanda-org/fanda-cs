using Fanda.Data.Commodity;
using System;
using System.Collections.Generic;

namespace Fanda.Data.Business
{
    public class PartyCategory
    {
        public Guid CategoryId { get; set; }
        public Guid OrgId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual ICollection<Party> Parties { get; set; }
        public virtual ICollection<ProductPricing> ProductPricings { get; set; }
    }
}