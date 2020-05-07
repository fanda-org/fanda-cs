using System.Collections.Generic;

namespace Fanda.Data
{
    public class ProductVariety : BaseOrgModel
    {
        public virtual ICollection<Product> Products { get; set; }
    }
}