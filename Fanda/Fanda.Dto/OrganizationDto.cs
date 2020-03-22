using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fanda.Dto
{
    public class OrganizationDto
    {
        public OrganizationDto()
        {
            Contacts = new HashSet<ContactDto>();
            Addresses = new HashSet<AddressDto>();
        }

        [Required]
        public Guid Id { get; set; }

        [StringLength(16)]
        public string OrgCode { get; set; }

        [StringLength(100)]
        public string OrgName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public string RegdNum { get; set; }
        public string PAN { get; set; }
        public string TAN { get; set; }
        public string GSTIN { get; set; }

        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual ICollection<ContactDto> Contacts { get; set; }
        public virtual ICollection<AddressDto> Addresses { get; set; }
        public virtual ICollection<UserDto> Users { get; set; }
    }
}