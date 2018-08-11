using Fanda.Common.Enums;
using Fanda.ViewModel.Base;
using System;
using System.Collections.Generic;

namespace Fanda.ViewModel.Business
{
    public class PartyViewModel
    {
        public Guid PartyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public string RegdNum { get; set; }
        public string PAN { get; set; }
        public string TAN { get; set; }
        public string GSTIN { get; set; }
        public PartyType PartyType { get; set; }
        public PaymentTerm PaymentTerm { get; set; }
        public decimal CreditLimit { get; set; }

        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public ICollection<ContactViewModel> Contacts { get; set; }
        public ICollection<AddressViewModel> Addresses { get; set; }
        //public ICollection<BankAccountViewModel> Banks { get; set; }
    }
}