using System;

namespace Fanda.ViewModel.Inventory
{
    public class InvoiceItemViewModel
    {
        public Guid InvItemId { get; set; }
        public Guid InvoiceId { get; set; }
        public string Description { get; set; }
        public Guid UnitId { get; set; }
        public decimal Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPct { get; set; }
        public decimal DiscountAmt { get; set; }
        public decimal CentralGstPct { get; set; }
        public decimal CentralGstAmt { get; set; }
        public decimal StateGstPct { get; set; }
        public decimal StateGstAmt { get; set; }
        public decimal InterGstPct { get; set; }
        public decimal InterGstAmt { get; set; }
        public decimal LineTotal { get; set; }

        public StockViewModel Stock { get; set; }
    }
}