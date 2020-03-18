using System;

namespace Fanda.Data.Models
{
    public class ProductIngredient
    {
        public Guid IngredientId { get; set; }
        public Guid ParentProductId { get; set; }
        public Guid ChildProductId { get; set; }
        public Guid UnitId { get; set; }
        public decimal Qty { get; set; }

        public virtual Product ParentProduct { get; set; }
        public virtual Product ChildProduct { get; set; }
        public virtual Unit Unit { get; set; }
    }
}