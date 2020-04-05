﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Fanda.Dto
{
    public class PartyCategoryDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Code"),
            Required(AllowEmptyStrings = false, ErrorMessage = "Code is required"),
            RegularExpression(@"\G(.+)[\t\r?\n]", ErrorMessage = @"The characters '\t' and '\n' are not allowed"),
            StringLength(16, ErrorMessage = "Maximum allowed length is 16")]
        public string Code { get; set; }

        [Display(Name = "Name"),
            Required(AllowEmptyStrings = false, ErrorMessage = "Category name is required"),
            RegularExpression(@"\G(.+)[\t\r?\n]", ErrorMessage = @"The characters '\t' and '\n' are not allowed"),            
            StringLength(50, ErrorMessage = "Maximum allowed length is 50")]
        public string Name { get; set; }

        [Display(Name = "Description"),
            RegularExpression(@"\G(.+)[\t\r?\n]", ErrorMessage = @"The characters '\t' and '\n' are not allowed"),
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
}