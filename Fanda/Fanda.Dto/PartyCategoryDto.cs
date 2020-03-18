using System;
using System.ComponentModel.DataAnnotations;

namespace Fanda.Dto
{
    public class PartyCategoryDto
    {
        //public string OrgId { get; set; }
        public string CategoryId { get; set; }

        [Display(Name = "Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Code is required")]
        [StringLength(16, ErrorMessage = "Maximum allowed length is 16")]
        public string Code { get; set; }

        [Display(Name = "Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Category name is required")]
        [StringLength(50, ErrorMessage = "Maximum allowed length is 50")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Code should not be empty")]
        [StringLength(255, ErrorMessage = "Maximum allowed length is 255")]
        public string Description { get; set; }

        public bool Active { get; set; }

        [Display(Name = "Date Created")]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Date Modified")]
        [DataType(DataType.DateTime)]
        public DateTime? DateModified { get; set; }
    }
}