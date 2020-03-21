using Fanda.Shared;

namespace Fanda.Dto
{
    public class ProductPricingRangeDto
    {
        public string PricingId { get; set; }
        public decimal MinQty { get; set; }
        public decimal MaxQty { get; set; }
        public decimal AdjustPct { get; set; }
        public decimal AdjustAmt { get; set; }
        public RoundOffOption RoundOffOption { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
