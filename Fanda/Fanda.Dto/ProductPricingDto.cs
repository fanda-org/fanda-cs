using System.Collections.Generic;

namespace Fanda.Dto
{
    public class ProductPricingDto
    {
        public ProductPricingDto()
        {
            PricingRanges = new HashSet<ProductPricingRangeDto>();
        }
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string PartyCategoryId { get; set; }
        public string InvoiceCategoryId { get; set; }

        public ICollection<ProductPricingRangeDto> PricingRanges { get; set; }
    }
}