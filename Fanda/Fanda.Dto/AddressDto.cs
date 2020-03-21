using Fanda.Shared;
using System.ComponentModel.DataAnnotations;

namespace Fanda.Dto
{
    public class AddressDto
    {
        public string Id { get; set; }

        [Display(Name = "Address Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Contact name should not be empty")]
        public AddressType AddressType { get; set; }

        [Display(Name = "Address Line 1")]
        [StringLength(100, ErrorMessage = "Maximum allowed length is 100")]
        public string AddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        [StringLength(100, ErrorMessage = "Maximum allowed length is 100")]
        public string AddressLine2 { get; set; }

        [StringLength(25, ErrorMessage = "Maximum allowed length is 25")]
        public string City { get; set; }

        [StringLength(25, ErrorMessage = "Maximum allowed length is 25")]
        public string State { get; set; }

        [StringLength(25, ErrorMessage = "Maximum allowed length is 25")]
        public string Country { get; set; }

        [Display(Name = "Postal code")]
        [StringLength(10, ErrorMessage = "Maximum allowed length is 10")]
        public string PostalCode { get; set; }

        //public bool IsDeleted { get; set; }
        //public int Index { get; set; }
    }
}