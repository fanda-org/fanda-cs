namespace Fanda.Core.Auth
{
    using System;
    using System.Collections.Generic;
    using Fanda.Core.Base;

    public class ApplicationDto : BaseDto
    {
        public string Edition { get; set; }
        public string Version { get; set; }

        public virtual ICollection<ResourceDto> Resources { get; set; }
    }

    public class ApplicationListDto : BaseListDto
    {
        public string Edition { get; set; }
        public string Version { get; set; }
    }

    public class AppChildrenDto
    {
        public virtual ICollection<ResourceDto> Resources { get; set; }
    }
}