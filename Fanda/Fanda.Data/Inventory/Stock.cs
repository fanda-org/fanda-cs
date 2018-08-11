using Fanda.Data.Commodity;
using System;
using System.Collections.Generic;

namespace Fanda.Data.Inventory
{
    public class Stock
    {
        public Guid StockId { get; set; }
        public Guid ProductId { get; set; }
        public string BatchNumber { get; set; }
        public DateTime? MfgDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public Guid UnitId { get; set; }
        public decimal QtyOnHand { get; set; }
        public virtual Product Product { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
    }
}