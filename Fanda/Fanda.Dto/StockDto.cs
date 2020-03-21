using System;

namespace Fanda.Dto
{
    public class StockDto
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string BatchNumber { get; set; }
        public DateTime? MfgDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string UnitId { get; set; }
        public decimal QtyOnHand { get; set; }
    }
}