using System;

namespace Fanda.Data.Models
{
    public class Device
    {
        public Guid DeviceId { get; set; }
        public Guid? LocationId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual Location Location { get; set; }
    }
}