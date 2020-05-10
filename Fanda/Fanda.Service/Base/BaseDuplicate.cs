using System;
using System.ComponentModel.DataAnnotations;

namespace Fanda.Service.Base
{
    public class Duplicate
    {
        [Required]
        public DuplicateField Field { get; set; }
        [Required]
        public string Value { get; set; }
        public Guid Id { get; set; } = default;
    }
    public class ChildDuplicate : Duplicate
    {
        public Guid ParentId { get; set; } = default;
    }
    public enum DuplicateField
    {
        Id = 1,
        Code = 2,
        Name = 3,
        Number = 4
    }
}
