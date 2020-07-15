namespace Fanda.Core.Auth
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ResourceActionDto
    {
        [Required]
        public Guid ResourceId { get; set; }
        [Required]
        public Guid ActionId { get; set; }

        public virtual ResourceDto Resource { get; set; }
        public virtual ActionDto Action { get; set; }
    }
}