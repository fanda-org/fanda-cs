namespace Fanda.Entities.Auth
{
    using System.Collections.Generic;

    public class Action : BaseParentModel
    {
        public virtual ICollection<ResourceAction> ResourceActions { get; set; }
    }
}