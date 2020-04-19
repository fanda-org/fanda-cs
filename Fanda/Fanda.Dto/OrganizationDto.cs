using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fanda.Dto
{
    public class OrganizationDto : BaseDto
    {
        public string RegdNum { get; set; }
        public string PAN { get; set; }
        public string TAN { get; set; }
        public string GSTIN { get; set; }

        public virtual ICollection<ContactDto> Contacts { get; set; }
        public virtual ICollection<AddressDto> Addresses { get; set; }
    }

    public class OrgListDto : BaseListDto { }

    public class OrgYearListDto : BaseListDto
    {
        public bool IsSelected { get; set; }
        public Guid SelectedYearId { get; set; }

        [Display(Name="Accounting Years")]
        public ICollection<YearListDto> AccountYears { get; set; }
    }
}