using System;
using System.Collections.Generic;

namespace Fanda.Data
{
    public class ProductCategory : BaseOrgModel
    {
        public Guid? ParentId { get; set; }

        public virtual ProductCategory Parent { get; set; }
        public virtual ICollection<ProductCategory> Children { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}