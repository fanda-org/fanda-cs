using System;
using System.ComponentModel.DataAnnotations;
using Fanda.Shared;

namespace Fanda.Infrastructure.Base
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
}
