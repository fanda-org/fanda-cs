using System;
using System.ComponentModel.DataAnnotations;

namespace Fanda.ViewModel.Access
{
    public class RoleViewModel
    {
        public string RoleId { get; set; }

        [StringLength(16)]
        public string Code { get; set; }

        [StringLength(25)]
        public string Name { get; set; }

        public string Description { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}