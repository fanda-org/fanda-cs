using System.Collections.Generic;

namespace Fanda.Data
{
    public class ProductBrand : BaseOrgModel
    {
        public virtual ICollection<Product> Products { get; set; }
    }
}