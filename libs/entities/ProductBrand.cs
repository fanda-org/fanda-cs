using System.Collections.Generic;

namespace Fanda.Entities
{
    public class ProductBrand : BaseOrgEntity
    {
        public virtual ICollection<Product> Products { get; set; }
    }
}