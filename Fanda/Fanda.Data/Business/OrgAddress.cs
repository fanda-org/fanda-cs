using Fanda.Data.Base;
using System;

namespace Fanda.Data.Business
{
    public class OrgAddress
    {
        public Guid OrgId { get; set; }
        public Guid AddressId { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual Address Address { get; set; }
    }
}