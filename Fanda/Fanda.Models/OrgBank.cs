using System;

namespace Fanda.Data
{
    public class OrgBank
    {
        public Guid OrgId { get; set; }
        public Guid BankAcctId { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual Bank BankAccount { get; set; }
    }
}