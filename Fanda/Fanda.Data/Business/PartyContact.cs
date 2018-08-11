using Fanda.Data.Base;
using System;

namespace Fanda.Data.Business
{
    public class PartyContact
    {
        public Guid PartyId { get; set; }
        public Guid ContactId { get; set; }

        public virtual Party Party { get; set; }
        public virtual Contact Contact { get; set; }
    }
}