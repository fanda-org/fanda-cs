using System.Collections.Generic;

namespace Fanda.Entities.Auth
{
    public class Application : BaseParentModel
    {
        public string Edition { get; set; }
        public string Version { get; set; }

        public virtual ICollection<AppResource> AppResources { get; set; }
    }
}