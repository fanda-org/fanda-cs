﻿using System;
using System.Collections.Generic;

namespace Fanda.Data
{
    public class AccountYear : BaseOrgModel
    {
        //public Guid Id { get; set; }
        //public string YearCode { get; set; }
        public DateTime YearBegin { get; set; }
        public DateTime YearEnd { get; set; }
        //public Guid OrgId { get; set; }

        //public virtual Organization Organization { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<LedgerBalance> LedgerBalances { get; set; }
    }
}
