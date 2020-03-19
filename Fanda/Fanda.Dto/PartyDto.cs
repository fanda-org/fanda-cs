using Fanda.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fanda.Dto
{
    public class PartyDto
    {
        public PartyDto()
        {
            Contacts = new HashSet<ContactDto>();
            Addresses = new HashSet<AddressDto>();
            Banks = new HashSet<BankAccountDto>();
        }

        //public string OrgId { get; set; }
        public string PartyId { get; set; }

        [Display(Name = "Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Code is required")]
        [StringLength(16, ErrorMessage = "Maximum allowed length is 16")]
        public string Code { get; set; }

        [Display(Name = "Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Maximum allowed length is 50")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select contact category")]
        [Display(Name = "Contact Category")]
        public string CategoryId { get; set; }

        public string CategoryName { get; set; }

        [Display(Name = "Regn.No.")]
        public string RegdNum { get; set; }

        public string PAN { get; set; }
        public string TAN { get; set; }
        public string GSTIN { get; set; }

        [Display(Name = "Contact Type")]
        public PartyType PartyType { get; set; }

        //public string PartyTypeString { get { return PartyType.ToString(); } }

        [Display(Name = "Payment Term")]
        public PaymentTerm PaymentTerm { get; set; }

        //public string PaymentTermString { get { return PaymentTerm.ToString(); } }

        [Display(Name = "Credit Limit")]
        public decimal CreditLimit { get; set; }

        public bool Active { get; set; }

        [Display(Name = "Date Created")]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Date Modified")]
        [DataType(DataType.DateTime)]
        public DateTime? DateModified { get; set; }

        public ICollection<ContactDto> Contacts { get; set; }
        public ICollection<AddressDto> Addresses { get; set; }
        public ICollection<BankAccountDto> Banks { get; set; }
    }
}