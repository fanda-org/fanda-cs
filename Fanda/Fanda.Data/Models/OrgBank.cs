using System;

namespace Fanda.Data.Models
{
    public class OrgBank
    {
        public Guid OrgId { get; set; }
        public Guid BankAcctId { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual BankAccount BankAccount { get; set; }
    }
}