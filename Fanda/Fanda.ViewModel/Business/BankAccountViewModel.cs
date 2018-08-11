using Fanda.Common.Enums;
using Fanda.ViewModel.Base;
using System;

namespace Fanda.ViewModel.Business
{
    public class BankAccountViewModel
    {
        public Guid BankAcctId { get; set; }
        public string AccountNumber { get; set; }
        public string BankShortName { get; set; }
        public string BankName { get; set; }
        public BankAccountType AccountType { get; set; }
        public string IfscCode { get; set; }
        public string MicrCode { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public virtual ContactViewModel Contact { get; set; }
        public virtual AddressViewModel Address { get; set; }

        public AccountOwner Owner { get; set; }
        public Guid? OwnerId { get; set; }
    }
}