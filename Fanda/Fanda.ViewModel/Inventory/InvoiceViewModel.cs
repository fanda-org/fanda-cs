using Fanda.Common.Enums;
using System;
using System.Collections.Generic;

namespace Fanda.ViewModel.Inventory
{
    public class InvoiceViewModel
    {
        public InvoiceViewModel()
        {
            InvoiceItems = new HashSet<InvoiceItemViewModel>();
        }
        public Guid InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public Guid CategoryId { get; set; }
        public InvoiceType InvoiceType { get; set; }
        public StockInvoiceType StockInvoiceType { get; set; }
        public Guid PartyId { get; set; }
        public string PartyRefNum { get; set; }
        public DateTime? PartyRefDate { get; set; }
        public Guid? BuyerId { get; set; }
        public string Notes { get; set; }
        public decimal Subtotal { get; set; }
        public decimal DiscountPct { get; set; }
        public decimal DiscountAmt { get; set; }
        public decimal TaxPct { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal MiscAddDesc { get; set; }
        public decimal MiscAddAmt { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public ICollection<InvoiceItemViewModel> InvoiceItems { get; set; }
    }
}