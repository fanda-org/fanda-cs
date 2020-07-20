namespace Fanda.Entities.Auth
{
    using System;

    public class Privilege
    {
        // Role
        public Guid RoleId { get; set; }
        // AppResource
        public Guid AppResourceId { get; set; }
        public bool Create { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public bool Read { get; set; }
        public bool Print { get; set; }
        public bool Import { get; set; }
        public bool Export { get; set; }
        #region Virtual members
        public virtual Role Role { get; set; }
        public virtual AppResource AppResource { get; set; }
        #endregion
    }
}