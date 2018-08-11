using Fanda.Data.Access;
using System;

namespace Fanda.Data.Business
{
    public class OrgUser
    {
        public Guid OrgId { get; set; }
        public Guid UserId { get; set; }
        public Guid? LocationId { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual User User { get; set; }
        public virtual Location Location { get; set; }
    }
}