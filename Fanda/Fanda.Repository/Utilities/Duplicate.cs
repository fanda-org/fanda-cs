using System;
using System.ComponentModel.DataAnnotations;

namespace Fanda.Repository.Utilities
{
    public class ParentDuplicate
    {
        [Required]
        public DuplicateField Field { get; set; }
        [Required]
        public string Value { get; set; }
        public Guid Id { get; set; } = default;
    }
    public class Duplicate : ParentDuplicate
    {
        public Guid ParentId { get; set; } = default;
    }
    public enum DuplicateField
    {
        Id = 1,
        Code = 2,
        Name = 3,
        Email = 4,
        Number = 5
    }
}
