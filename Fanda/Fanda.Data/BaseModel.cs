using System;

namespace Fanda.Data
{
    public class RootModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }

    public class BaseModel : RootModel
    {        
        public string Code { get; set; }        
        public string Description { get; set; }
    }

    public class BaseOrgModel : BaseModel
    {
        public Guid OrgId { get; set; }
        public virtual Organization Organization { get; set; }
    }

    public class BaseYearModel
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public Guid YearId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public virtual AccountYear AccountYear { get; set; }
    }
}
