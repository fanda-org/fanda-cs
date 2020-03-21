using System;

namespace Fanda.Data
{
    public class LedgerBalance
    {
        public Guid LedgerId { get; set; }
        public Guid YearId { get; set; }
        public decimal OpeningBalance { get; set; }
        public string BalanceSign { get; set; }

        public virtual Ledger Ledger { get; set; }
        public virtual AccountYear AccountYear { get; set; }
    }
}
