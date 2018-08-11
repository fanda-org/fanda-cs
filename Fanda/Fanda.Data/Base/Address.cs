using Fanda.Common.Enums;
using Fanda.Data.Business;
using System;
using System.Collections.Generic;

namespace Fanda.Data.Base
{
    public class Address
    {
        public Guid AddressId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public AddressType AddressType { get; set; }

        public string AddressTypeString
        {
            get { return AddressType.ToString(); }
            set { AddressType = (AddressType)Enum.Parse(typeof(AddressType), value, true); }
        }

        public virtual BankAccount BankAccount { get; set; }
        public virtual ICollection<OrgAddress> OrgAddresses { get; set; }

        //public virtual ICollection<BankAddress> BankAddresses { get; set; }
        public virtual ICollection<PartyAddress> PartyAddresses { get; set; }
    }
}