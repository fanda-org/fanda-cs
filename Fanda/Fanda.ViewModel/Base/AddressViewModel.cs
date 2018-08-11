using Fanda.Common.Enums;
using System;

namespace Fanda.ViewModel.Base
{
    public class AddressViewModel
    {
        public Guid AddressId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Postalcode { get; set; }
        public AddressType AddressType { get; set; }
    }
}