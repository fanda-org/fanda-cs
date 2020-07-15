namespace Fanda.Entities.Auth
{
    using System;

    public class Privilege
    {
        // Role
        public Guid RoleId { get; set; }

        // AppResource
        public Guid AppResourceId { get; set; }
        // ResourceAction
        public Guid ResourceActionId { get; set; }

        public virtual Role Role { get; set; }
        public virtual AppResource AppResource { get; set; }
        public virtual ResourceAction ResourceAction { get; set; }
    }
}