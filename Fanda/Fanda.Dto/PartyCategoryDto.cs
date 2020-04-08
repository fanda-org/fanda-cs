using System;
using System.ComponentModel.DataAnnotations;

namespace Fanda.Dto
{
    public class PartyCategoryDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Code"),
            Required(AllowEmptyStrings = false, ErrorMessage = "Code is required"),
            RegularExpression(@"^[a-zA-Z0-9~!@#$()_+-{}|:<>.?\/]+$", ErrorMessage = @"Space or tab are not allowed in code"),
            StringLength(16, ErrorMessage = "Maximum allowed length is 16")]
        public string Code { get; set; }

        [Display(Name = "Name"),
            Required(AllowEmptyStrings = false, ErrorMessage = "Category name is required"),
            RegularExpression(@"^[a-zA-Z0-9\s~!@#$()_+-{}|:<>.?\/]*$", ErrorMessage = @"Special characters are not allowed in name"),
            StringLength(50, ErrorMessage = "Maximum allowed length is 50")]
        public string Name { get; set; }

        [Display(Name = "Description"),
            RegularExpression(@"^[a-zA-Z0-9\s~!@#$()_+-{}|:<>.?\/]*$", ErrorMessage = @"Special characters are not allowed in description"),
            StringLength(255, ErrorMessage = "Maximum allowed length is 255")]
        public string Description { get; set; }

        public bool Active { get; set; }

        [Display(Name = "Date Created")]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Date Modified")]
        [DataType(DataType.DateTime)]
        public DateTime? DateModified { get; set; }
    }
    //[RegularExpression(@"^[^\\/:*;\.\)\(]+$", ErrorMessage = "The characters ':', '.' ';', '*', '/' and '\' are not allowed")]
    // "^[a-zA-Z0-9~`!@#$%^&*()_+-={}|:;<>,.?\/']+$"
    // "^((?![\s{2,}|\t]).)*$"
}