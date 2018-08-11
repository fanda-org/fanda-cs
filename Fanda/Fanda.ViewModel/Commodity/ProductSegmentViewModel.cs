using System;

namespace Fanda.ViewModel.Commodity
{
    public class ProductSegmentViewModel
    {
        public Guid SegmentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}