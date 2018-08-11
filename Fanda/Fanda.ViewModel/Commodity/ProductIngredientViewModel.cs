using System;

namespace Fanda.ViewModel.Commodity
{
    public class ProductIngredientViewModel
    {
        public Guid IngredientId { get; set; }
        public Guid ParentProductId { get; set; }
        public Guid ChildProductId { get; set; }
        public Guid UnitId { get; set; }
        public decimal Qty { get; set; }
    }
}