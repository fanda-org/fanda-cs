﻿using Fanda.Dto.Base;
using System;

namespace Fanda.Dto
{
    public class AccountYearDto : BaseDto
    {
        //public Guid Id { get; set; }
        //public string YearCode { get; set; }
        public DateTime YearBegin { get; set; }
        public DateTime YearEnd { get; set; }
    }

    public class YearListDto : BaseListDto
    {
    }
}
