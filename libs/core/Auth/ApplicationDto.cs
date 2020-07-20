namespace Fanda.Core.Auth
{
    using System;
    using System.Collections.Generic;
    using Fanda.Core.Base;

    public class ApplicationDto : BaseDto
    {
        public string Edition { get; set; }
        public string Version { get; set; }

        public ICollection<AppResourceDto> AppResources { get; set; }
    }

    public class ApplicationListDto : BaseListDto
    {
        public string Edition { get; set; }
        public string Version { get; set; }
    }

    public class AppChildrenDto
    {
        public ICollection<AppResourceDto> AppResources { get; set; }
    }
}