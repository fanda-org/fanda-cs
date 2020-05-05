using System;
using System.ComponentModel.DataAnnotations;

namespace Fanda.Service.Base
{
    //public class RootDuplicate
    //{
    //    [Required]
    //    public DuplicateField Field { get; set; }
    //    [Required]
    //    public string Value { get; set; }
    //    public Guid Id { get; set; } = default;
    //}
    public class BaseDuplicate
    {
        [Required]
        public DuplicateField Field { get; set; }
        [Required]
        public string Value { get; set; }
        public Guid Id { get; set; } = default;
    }
    public class BaseOrgDuplicate : BaseDuplicate
    {
        public Guid OrgId { get; set; } = default;
    }
    public class BaseYearDuplicate : BaseDuplicate
    {
        public Guid YearId { get; set; } = default;
    }
    public enum DuplicateField
    {
        Id = 1,
        Code = 2,
        Name = 3,
        Number = 4
    }
}
