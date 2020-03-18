using System;
using System.ComponentModel.DataAnnotations;

namespace Fanda.Dto
{
    public class ContactDto
    {
        public Guid ContactId { get; set; }

        [Display(Name = "Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Contact name is required")]
        [StringLength(50, ErrorMessage = "Maximum allowed length is 50")]
        public string ContactName { get; set; }

        [Display(Name = "Title")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Contact title should not be empty")]
        [StringLength(50, ErrorMessage = "Maximum allowed length is 50")]
        public string ContactTitle { get; set; }

        [Display(Name = "Phone")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Contact phone should not be empty")]
        [StringLength(25, ErrorMessage = "Maximum allowed length is 25")]
        public string ContactPhone { get; set; }

        [Display(Name = "Email")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Contact email should not be empty")]
        [StringLength(255, ErrorMessage = "Maximum allowed length is 255")]
        public string ContactEmail { get; set; }

        public bool IsDeleted { get; set; }
        public int Index { get; set; }
    }
}