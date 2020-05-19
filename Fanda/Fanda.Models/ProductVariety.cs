using System.Collections.Generic;

namespace Fanda.Models
{
    public class ProductVariety : BaseOrgModel
    {
        public virtual ICollection<Product> Products { get; set; }
    }
}