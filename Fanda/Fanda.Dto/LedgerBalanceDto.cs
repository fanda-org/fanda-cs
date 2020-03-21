namespace Fanda.Dto
{
    public class LedgerBalanceDto
    {
        public string LedgerId { get; set; }
        public string YearId { get; set; }
        public decimal OpeningBalance { get; set; }
        public string BalanceSign { get; set; }

        //public virtual LedgerDto Ledger { get; set; }
        //public virtual AccountYearDto AccountYear { get; set; }
    }
}
