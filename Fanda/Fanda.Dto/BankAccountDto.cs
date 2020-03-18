using Fanda.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fanda.Dto
{
    public class BankAccountDto
    {
        public Guid BankAcctId { get; set; }

        [Display(Name = "Account No.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Account No. is required")]
        [StringLength(25, ErrorMessage = "Maximum allowed length is 25")]
        public string AccountNumber { get; set; }

        [Display(Name = "Short Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Short name is required")]
        [StringLength(15, ErrorMessage = "Maximum allowed length is 15")]
        public string BankShortName { get; set; }

        [Display(Name = "Bank Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Bank name is required")]
        [StringLength(50, ErrorMessage = "Maximum allowed length is 50")]
        public string BankName { get; set; }

        [Display(Name = "Account Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Account Type is required")]
        [StringLength(16, ErrorMessage = "Maximum allowed length is 16")]
        public BankAccountType AccountType { get; set; }

        [Display(Name = "IFSC")]
        [StringLength(16, ErrorMessage = "Maximum allowed length is 16")]
        public string IfscCode { get; set; }

        [Display(Name = "MICR")]
        [StringLength(16, ErrorMessage = "Maximum allowed length is 16")]
        public string MicrCode { get; set; }

        [Display(Name = "Branch Code")]
        [StringLength(16, ErrorMessage = "Maximum allowed length is 16")]
        public string BranchCode { get; set; }

        [Display(Name = "Branch Name")]
        [StringLength(50, ErrorMessage = "Maximum allowed length is 50")]
        public string BranchName { get; set; }

        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public ContactDto Contact { get; set; }
        public AddressDto Address { get; set; }

        public AccountOwner Owner { get; set; }
        public Guid? OwnerId { get; set; }

        public bool IsDeleted { get; set; }
        public int Index { get; set; }
    }
}