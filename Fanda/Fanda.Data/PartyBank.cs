using System;

namespace Fanda.Data
{
    public class PartyBank
    {
        public Guid PartyId { get; set; }
        public Guid BankAcctId { get; set; }

        public virtual Party Party { get; set; }
        public virtual BankAccount BankAccount { get; set; }
    }
}