using System.Collections.Generic;

namespace Fanda.Models
{
    public class ProductBrand : BaseOrgModel
    {
        public virtual ICollection<Product> Products { get; set; }
    }
}