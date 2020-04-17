﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Fanda.Dto
{
    public class BaseDto
    {
        [Required]
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
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }

    public class BaseYearDto
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}