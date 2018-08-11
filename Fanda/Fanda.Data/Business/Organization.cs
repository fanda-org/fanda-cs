using Fanda.Data.Commodity;
using Fanda.Data.Inventory;
using System;
using System.Collections.Generic;

namespace Fanda.Data.Business
{
    public class Organization
    {
        public Guid OrgId { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }
        public string Description { get; set; }
        public string RegdNum { get; set; }
        public string PAN { get; set; }
        public string TAN { get; set; }
        public string GSTIN { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public virtual ICollection<OrgContact> Contacts { get; set; }
        public virtual ICollection<OrgAddress> Addresses { get; set; }

        public virtual ICollection<OrgUser> Users { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<OrgBank> Banks { get; set; }
        public virtual ICollection<PartyCategory> PartyCategories { get; set; }
        public virtual ICollection<ProductBrand> ProductBrands { get; set; }
        public virtual ICollection<ProductSegment> ProductSegments { get; set; }
        public virtual ICollection<ProductVariety> ProductVarieties { get; set; }
        public virtual ICollection<Party> Parties { get; set; }
        public virtual ICollection<Unit> Units { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<InvoiceCategory> InvoiceCategories { get; set; }
        public virtual ICollection<UnitConversion> UnitConversions { get; set; }
    }
}