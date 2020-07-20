using System.Collections.Generic;

namespace Fanda.Entities
{
    public class ProductVariety : BaseOrgEntity
    {
        public virtual ICollection<Product> Products { get; set; }
    }
}