using System;

namespace Fanda.Dto
{
    public class LedgerDto
    {
        public string Id { get; set; }
        public string LedgerCode { get; set; }
        public string LedgerName { get; set; }
        public string Description { get; set; }
        public string LedgerGroupId { get; set; }
        public string LedgerGroupName { get; set; }
        public string ParentId { get; set; }
        public bool IsSystem { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public virtual LedgerBalanceDto LedgerBalance { get; set; }
    }
}
