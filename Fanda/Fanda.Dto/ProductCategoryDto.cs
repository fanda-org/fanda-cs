using Fanda.Dto.Base;
using System;

namespace Fanda.Dto
{
    public class ProductCategoryDto : BaseDto
    {
        public Guid? ParentId { get; set; }
    }
    public class ProductCategoryListDto : BaseListDto
    {
        public string ParentName { get; set; }
    }
}