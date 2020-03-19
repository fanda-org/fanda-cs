using Fanda.Shared;
using System;

namespace Fanda.Data.Models
{
    public class AuditTrail
    {
        public Guid AuditTrailId { get; set; }
        public string TableName { get; set; }
        public Guid RowId { get; set; }
        public Status CurrentStatus { get; set; }

        public string CurrentStatusString
        {
            get { return CurrentStatus.ToString(); }
            set { CurrentStatus = (Status)Enum.Parse(typeof(Status), value, true); }
        }

        public Guid CreatedUserId { get; set; }
        public DateTime DateCreated { get; set; }

        public Guid? ModifiedUserId { get; set; }
        public DateTime? DateModified { get; set; }

        public Guid? DeletedUserId { get; set; }
        public DateTime? DateDeleted { get; set; }

        public Guid? PrintedUserId { get; set; }
        public DateTime? DatePrinted { get; set; }

        public Guid? ApprovedUserId { get; set; }
        public DateTime? DateApproved { get; set; }

        public Guid? RejectedUserId { get; set; }
        public DateTime? DateRejected { get; set; }

        public Guid? HoldUserId { get; set; }
        public DateTime? DateHold { get; set; }

        public Guid? ActivatedUserId { get; set; }
        public DateTime? DateActivated { get; set; }

        public Guid? DeactivatedUserId { get; set; }
        public DateTime? DateDeactivated { get; set; }

        public virtual User CreatedUser { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual User DeletedUser { get; set; }
        public virtual User PrintedUser { get; set; }
        public virtual User ApprovedUser { get; set; }
        public virtual User RejectedUser { get; set; }
        public virtual User HoldUser { get; set; }
        public virtual User ActivatedUser { get; set; }
        public virtual User DeactivatedUser { get; set; }
    }
}