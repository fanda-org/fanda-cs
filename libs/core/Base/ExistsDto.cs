using System.ComponentModel.DataAnnotations;
using Fanda.Shared;

namespace Fanda.Core.Base
{
    public class ExistsDto
    {
        [Required]
        public DuplicateField Field { get; set; }
        [Required]
        public string Value { get; set; }
    }
}