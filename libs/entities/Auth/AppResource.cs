namespace Fanda.Entities.Auth
{
    using System;
    using System.Collections.Generic;
    using Fanda.Shared;

    public class AppResource : BaseEntity
    {
        public ResourceType ResourceType { get; set; }
        public string ResourceTypeString
        {
            get { return ResourceType.ToString(); }
            set { ResourceType = (ResourceType)Enum.Parse(typeof(ResourceType), value, true); }
        }
        public Guid ApplicationId { get; set; }
        #region Action fields
        public bool Creatable { get; set; }
        public bool Updateable { get; set; }
        public bool Deleteable { get; set; }
        public bool Readable { get; set; }
        public bool Printable { get; set; }
        public bool Importable { get; set; }
        public bool Exportable { get; set; }
        #endregion
        public virtual Application Application { get; set; }
        public virtual ICollection<Privilege> Privileges { get; set; }
    }
}