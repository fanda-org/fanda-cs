namespace Fanda.Core.Auth
{
    using System;
    using Fanda.Core.Base;
    using Fanda.Shared;

    public class ResourceDto : BaseDto
    {
        public ResourceType ResourceType { get; set; }
        public string ResourceTypeString
        {
            get { return ResourceType.ToString(); }
            set { ResourceType = (ResourceType)Enum.Parse(typeof(ResourceType), value, true); }
        }
    }

    public class ResourceListDto : BaseListDto
    {
        public ResourceType ResourceType { get; set; }
    }
}