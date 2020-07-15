namespace Fanda.Core.Auth
{
    using System;

    public class AppResourceDto
    {
        public Guid ApplicationId { get; set; }
        public Guid ResourceId { get; set; }

        public virtual ApplicationDto Application { get; set; }
        public virtual ResourceDto Resource { get; set; }
    }
}