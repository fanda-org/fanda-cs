using Fanda.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fanda.ViewModel.Commodity
{
    public class ProductPricingViewModel
    {
        public Guid PricingId { get; set; }
        public Guid ProductId { get; set; }
        public Guid? PartyCategoryId { get; set; }
        public Guid? InvoiceCategoryId { get; set; }

        public ICollection<ProductPricingRangeViewModel> PricingRanges { get; set; }
    }

    public class ProductPricingRangeViewModel
    {
        public Guid PricingId { get; set; }
        public Guid RangeId { get; set; }
        public decimal MinQty { get; set; }
        public decimal MaxQty { get; set; }
        public decimal AdjustPct { get; set; }
        public decimal AdjustAmt { get; set; }
        public RoundOffOption RoundOffOption { get; set; }
        public decimal FinalPrice { get; set; }
    }
}