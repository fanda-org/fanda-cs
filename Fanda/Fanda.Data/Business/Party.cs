using Fanda.Common.Enums;
using Fanda.Data.Inventory;
using System;
using System.Collections.Generic;

namespace Fanda.Data.Business
{
    public class Party
    {
        public Guid PartyId { get; set; }
        public Guid OrgId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public string RegdNum { get; set; }
        public string PAN { get; set; }
        public string TAN { get; set; }
        public string GSTIN { get; set; }

        public PartyType PartyType { get; set; }

        public string PartyTypeString
        {
            get { return PartyType.ToString(); }
            set { PartyType = (PartyType)Enum.Parse(typeof(PartyType), value, true); }
        }

        public PaymentTerm PaymentTerm { get; set; }

        public string PaymentTermString
        {
            get { return PaymentTerm.ToString(); }
            set { PaymentTerm = (PaymentTerm)Enum.Parse(typeof(PaymentTerm), value, true); }
        }

        public decimal CreditLimit { get; set; }

        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual PartyCategory Category { get; set; }
        public virtual ICollection<PartyContact> Contacts { get; set; }
        public virtual ICollection<PartyAddress> Addresses { get; set; }
        public virtual ICollection<PartyBank> Banks { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}