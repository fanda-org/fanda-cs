﻿namespace Fanda.Dto
{
    public class ProductIngredientDto
    {
        public string Id { get; set; }
        public string ParentProductId { get; set; }
        public string ChildProductId { get; set; }
        public string UnitId { get; set; }
        public decimal Qty { get; set; }
    }
}