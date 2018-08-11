using Fanda.Common.Enums;
using Fanda.Data.Business;
using Fanda.Data.Inventory;
using System;
using System.Collections.Generic;

namespace Fanda.Data.Commodity
{
    public class ProductPricing
    {
        public Guid PricingId { get; set; }
        public Guid ProductId { get; set; }
        public Guid? PartyCategoryId { get; set; }
        public Guid? InvoiceCategoryId { get; set; }

        public virtual Product Product { get; set; }
        public virtual PartyCategory PartyCategory { get; set; }
        public virtual InvoiceCategory InvoiceCategory { get; set; }
        public virtual ICollection<ProductPricingRange> PricingRanges { get; set; }
    }

    public class ProductPricingRange
    {
        public Guid RangeId { get; set; }
        public Guid PricingId { get; set; }
        public decimal MinQty { get; set; }
        public decimal MaxQty { get; set; }
        public decimal AdjustPct { get; set; }
        public decimal AdjustAmt { get; set; }
        public RoundOffOption RoundOffOption { get; set; }

        public string RoundOffOptionString
        {
            get { return RoundOffOption.ToString(); }
            set { RoundOffOption = (RoundOffOption)Enum.Parse(typeof(RoundOffOption), value, true); }
        }

        public decimal FinalPrice { get; set; }

        public virtual ProductPricing ProductPricing { get; set; }
    }
}