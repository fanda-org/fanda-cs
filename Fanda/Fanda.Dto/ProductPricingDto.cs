using System;
using System.Collections.Generic;

namespace Fanda.Dto
{
    public class ProductPricingDto
    {
        public Guid PricingId { get; set; }
        public Guid ProductId { get; set; }
        public Guid? PartyCategoryId { get; set; }
        public Guid? InvoiceCategoryId { get; set; }

        public ICollection<ProductPricingRangeDto> PricingRanges { get; set; }
    }
}