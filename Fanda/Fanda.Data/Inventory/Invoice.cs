using Fanda.Common.Enums;
using Fanda.Data.Business;
using System;
using System.Collections.Generic;

namespace Fanda.Data.Inventory
{
    public class Invoice
    {
        public Guid InvoiceId { get; set; }
        public Guid OrgId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public Guid CategoryId { get; set; }

        public InvoiceType InvoiceType { get; set; }

        public string InvoiceTypeString
        {
            get { return InvoiceType.ToString(); }
            set { InvoiceType = (InvoiceType)Enum.Parse(typeof(InvoiceType), value, true); }
        }

        public StockInvoiceType StockInvoiceType { get; set; }

        public string StockInvoiceTypeString
        {
            get { return StockInvoiceType.ToString(); }
            set { StockInvoiceType = (StockInvoiceType)Enum.Parse(typeof(StockInvoiceType), value, true); }
        }

        public string Notes { get; set; }
        public Guid PartyId { get; set; }
        public string PartyRefNum { get; set; }
        public DateTime? PartyRefDate { get; set; }

        // Trailer
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

        // Virtual members
        public virtual Organization Organization { get; set; }

        public virtual InvoiceCategory Category { get; set; }
        public virtual Party Party { get; set; }
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
    }
}