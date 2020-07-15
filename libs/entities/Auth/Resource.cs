namespace Fanda.Entities.Auth
{
    using System;
    using System.Collections.Generic;
    using Fanda.Shared;

    public class Resource : BaseParentModel
    {
        public ResourceType ResourceType { get; set; }
        public string ResourceTypeString
        {
            get { return ResourceType.ToString(); }
            set { ResourceType = (ResourceType)Enum.Parse(typeof(ResourceType), value, true); }
        }
        public virtual ICollection<ResourceAction> ResourceActions { get; set; }
        public virtual ICollection<AppResource> AppResources { get; set; }
    }
}