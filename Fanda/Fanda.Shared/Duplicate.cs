using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fanda.Shared
{
    public class Duplicate
    {
        [Required]
        public DuplicateField Field { get; set; }
        [Required]
        public string Value { get; set; }
        public Guid Id { get; set; } = default;
        public Guid OrgId { get; set; } = default;
    }

    public enum DuplicateField
    {
        Code = 1,
        Name = 2
    }
}
