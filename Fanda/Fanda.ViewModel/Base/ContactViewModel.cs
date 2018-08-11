using System;

namespace Fanda.ViewModel.Base
{
    public class ContactViewModel
    {
        public Guid ContactId { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
    }
}