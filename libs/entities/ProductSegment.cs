using System.Collections.Generic;

namespace Fanda.Entities
{
    public class ProductSegment : BaseOrgModel
    {
        public virtual ICollection<Product> Products { get; set; }
    }
}