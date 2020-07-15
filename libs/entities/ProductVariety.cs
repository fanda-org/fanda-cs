using System.Collections.Generic;

namespace Fanda.Entities
{
    public class ProductVariety : BaseOrgModel
    {
        public virtual ICollection<Product> Products { get; set; }
    }
}