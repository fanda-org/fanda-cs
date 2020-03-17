using Fanda.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fanda.ViewModel.Business
{
    public class OrganizationViewModel
    {
        public OrganizationViewModel()
        {
            Contacts = new HashSet<ContactViewModel>();
            Addresses = new HashSet<AddressViewModel>();
            Banks = new HashSet<BankAccountViewModel>();
        }

        [Required]
        public string OrgId { get; set; }

        [StringLength(16)]
        public string OrgCode { get; set; }

        [StringLength(100)]
        public string OrgName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public string RegdNum { get; set; }
        public string PAN { get; set; }
        public string TAN { get; set; }
        public string GSTIN { get; set; }

        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public ICollection<ContactViewModel> Contacts { get; set; }
        public ICollection<AddressViewModel> Addresses { get; set; }
        public ICollection<BankAccountViewModel> Banks { get; set; }
    }
}