using System;
using System.ComponentModel.DataAnnotations;

namespace Fanda.Dto
{
    public class BaseDto
    {
        [Required]
        public Guid Id { get; set; }

        [Display(Name = "Code", Prompt = "Code"),
            Required(AllowEmptyStrings = false, ErrorMessage = "Code is required"),
            RegularExpression(@"^[a-zA-Z0-9~!@#$()_+-{}|:<>.?\/]+$", ErrorMessage = @"Space or tab are not allowed in code"),
            StringLength(16, ErrorMessage = "Maximum allowed length is 16")]
        public string Code { get; set; }

        [Display(Name = "Name"),
            Required(AllowEmptyStrings = false, ErrorMessage = "Name is required"),
            RegularExpression(@"^[a-zA-Z0-9\s~!@#$()_+-{}|:<>.?\/]*$", ErrorMessage = @"Special characters are not allowed in name"),
            StringLength(50, ErrorMessage = "Maximum allowed length is 50")]
        public string Name { get; set; }

        [Display(Name = "Description"),
            RegularExpression(@"^[a-zA-Z0-9\s~!@#$()_+-{}|:<>.?\/]*$", ErrorMessage = @"Special characters are not allowed in description"),
            StringLength(255, ErrorMessage = "Maximum allowed length is 255")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string Description { get; set; }

        public bool? Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }

    public class BaseListDto
    {
        [Required]
        public Guid Id { get; set; }

        [Display(Name = "Code")]
        public string Code { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
        public bool? Active { get; set; }
    }

    public class BaseYearDto
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }

    public class BaseListYearDto
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string ContactFullName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactMobile { get; set; }
        public string AddressCity { get; set; }
        public decimal NetAmount { get; set; }
    }
}
