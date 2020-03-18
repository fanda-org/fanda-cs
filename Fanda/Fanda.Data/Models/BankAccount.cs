using Fanda.Shared.Enums;
using System;
using System.Collections.Generic;

namespace Fanda.Data.Models
{
    public class BankAccount
    {
        public Guid BankAcctId { get; set; }
        public string AccountNumber { get; set; }
        public string BankShortName { get; set; }
        public string BankName { get; set; }
        public BankAccountType AccountType { get; set; }

        public string AccountTypeString
        {
            get { return AccountType.ToString(); }
            set { AccountType = (BankAccountType)Enum.Parse(typeof(BankAccountType), value, true); }
        }

        public string IfscCode { get; set; }
        public string MicrCode { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public Guid? ContactId { get; set; }
        public Guid? AddressId { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual Contact Contact { get; set; }
        public virtual Address Address { get; set; }
        //public virtual ICollection<BankContact> Contacts { get; set; }
        //public virtual ICollection<BankAddress> Addresses { get; set; }

        public virtual ICollection<OrgBank> OrgBanks { get; set; }
        public virtual ICollection<PartyBank> PartyBanks { get; set; }
    }
}