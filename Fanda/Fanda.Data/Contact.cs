using System;
using System.Collections.Generic;

namespace Fanda.Data
{
    public class Contact
    {
        public Guid ContactId { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }

        public virtual BankAccount BankAccount { get; set; }
        public virtual ICollection<OrgContact> OrgContacts { get; set; }

        //public virtual ICollection<BankContact> BankContacts { get; set; }
        public virtual ICollection<PartyContact> PartyContacts { get; set; }
    }
}