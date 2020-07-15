using System;
using System.ComponentModel.DataAnnotations;

namespace Fanda.Core.Base
{
    public class ActiveStatus
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public bool Active { get; set; }
    }
}