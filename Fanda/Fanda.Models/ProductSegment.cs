using System.Collections.Generic;

namespace Fanda.Models
{
    public class ProductSegment : BaseOrgModel
    {
        public virtual ICollection<Product> Products { get; set; }
    }
}