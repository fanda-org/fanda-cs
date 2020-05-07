using System.Collections.Generic;

namespace Fanda.Data
{
    public class ProductSegment : BaseOrgModel
    {
        public virtual ICollection<Product> Products { get; set; }
    }
}