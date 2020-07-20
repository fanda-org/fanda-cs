using System.Collections.Generic;

namespace Fanda.Entities
{
    public class ProductSegment : BaseOrgEntity
    {
        public virtual ICollection<Product> Products { get; set; }
    }
}