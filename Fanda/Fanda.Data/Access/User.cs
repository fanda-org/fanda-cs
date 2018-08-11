using Fanda.Data.Business;
using Fanda.Data.Tracking;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Fanda.Data.Access
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateLastLogin { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool Active { get; set; }

        //public virtual ICollection<IdentityUserRole<Guid>> Roles { get; set; }
        public virtual ICollection<OrgUser> Organizations { get; set; }

        public virtual ICollection<AuditTrail> CreatedTrails { get; set; }
        public virtual ICollection<AuditTrail> ModifiedTrails { get; set; }
        public virtual ICollection<AuditTrail> DeletedTrails { get; set; }
        public virtual ICollection<AuditTrail> PrintedTrails { get; set; }
        public virtual ICollection<AuditTrail> ApprovedTrails { get; set; }
        public virtual ICollection<AuditTrail> RejectedTrails { get; set; }
        public virtual ICollection<AuditTrail> HoldTrails { get; set; }
        public virtual ICollection<AuditTrail> ActivatedTrails { get; set; }
        public virtual ICollection<AuditTrail> DeactivatedTrails { get; set; }

        public User() : base()
        {
            DateCreated = DateTime.Now;
            Active = true;
        }

        public User(string userName) : base(userName)
        {
            DateCreated = DateTime.Now;
            Active = true;
        }
    }
}