using Fanda.Shared;
using System;

namespace Fanda.Dto
{
    public class LedgerGroupDto
    {
        public string Id { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public LedgerGroupType GroupType { get; set; }
        public string ParentId { get; set; }
        public bool IsSystem { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
