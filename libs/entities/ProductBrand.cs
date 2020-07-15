using System.Collections.Generic;

namespace Fanda.Entities
{
    public class ProductBrand : BaseOrgModel
    {
        public virtual ICollection<Product> Products { get; set; }
    }
}