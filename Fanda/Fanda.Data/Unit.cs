using System;
using System.Collections.Generic;

namespace Fanda.Data
{
    public class Unit
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid OrgId { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
        public virtual ICollection<Stock> Stocks { get; set; }
        public virtual ICollection<ProductIngredient> ProductIngredients { get; set; }
        public virtual ICollection<UnitConversion> FromUnitConversions { get; set; }
        public virtual ICollection<UnitConversion> ToUnitConversions { get; set; }
    }
}