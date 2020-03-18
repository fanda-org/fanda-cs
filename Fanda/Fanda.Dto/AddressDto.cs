using Fanda.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fanda.Dto
{
    public class AddressDto
    {
        public Guid AddressId { get; set; }

        [Display(Name = "Address Line 1")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Contact name should not be empty")]
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

        [StringLength(10, ErrorMessage = "Maximum allowed length is 10")]
        public string Postalcode { get; set; }

        public AddressType AddressType { get; set; }

        public bool IsDeleted { get; set; }
        public int Index { get; set; }
    }
}