using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Fanda.Shared.Enums
{
    public static class FandaEnums
    {
        //public static IEnumerable<RoleType> GetRoleTypes() => Enum.GetValues(typeof(RoleType)) as IEnumerable<RoleType>;
        public static IEnumerable<AddressType> GetAddressTypes() => Enum.GetValues(typeof(AddressType)) as IEnumerable<AddressType>;

        public static IEnumerable<BankAccountType> GetBankAccountTypes() => Enum.GetValues(typeof(BankAccountType)) as IEnumerable<BankAccountType>;

        public static IEnumerable<PaymentTerm> GetPaymentTerms() => Enum.GetValues(typeof(PaymentTerm)) as IEnumerable<PaymentTerm>;

        public static IEnumerable<PartyType> GetPartyTypes() => Enum.GetValues(typeof(PartyType)) as IEnumerable<PartyType>;

        public static IEnumerable<ProductType> GetProductTypes() => Enum.GetValues(typeof(ProductType)) as IEnumerable<ProductType>;

        public static IEnumerable<InvoiceType> GetInvoiceTypes() => Enum.GetValues(typeof(InvoiceType)) as IEnumerable<InvoiceType>;

        public static IEnumerable<StockInvoiceType> GetStockInvoiceTypes() => Enum.GetValues(typeof(StockInvoiceType)) as IEnumerable<StockInvoiceType>;

        public static IEnumerable<Status> GetStatus() => Enum.GetValues(typeof(Status)) as IEnumerable<Status>;

        public static IEnumerable<RoundOffOption> GetRoundOffOptions() => Enum.GetValues(typeof(RoundOffOption)) as IEnumerable<RoundOffOption>;
    }

    //public enum RoleType
    //{
    //    Guest = 0,
    //    User = 1,
    //    PowerUser = 2,
    //    Manager = 4,
    //    Administrator = 8,
    //    SuperAdmin = 16
    //}

    public enum AccountOwner
    {
        None = 0,
        Organization = 1,
        Party = 2
    }

    public enum AddressType
    {
        Default = 0,
        Billing = 1,
        Shipping = 2,
        Remittance = 3
    }

    public enum BankAccountType
    {
        Default = 0,
        Savings = 1,
        Current = 2,
        Fixed = 3,
        Demat = 4,
        Salary = 5
    }

    public enum PaymentTerm
    {
        None = 0,
        OnReceipt = 1,
        Net7 = 2,
        Net10 = 2,
        Net30 = 3,
        Net60 = 4,
        Net90 = 5,
        OnDate = 6
    }

    public enum PartyType
    {
        [EnumMember(Value = "Customer")]
        Customer = 1,

        [EnumMember(Value = "Supplier")]
        Supplier = 2,

        [EnumMember(Value = "Buyer")]
        Buyer = 3,

        [EnumMember(Value = "Other")]
        Other = 4
    }

    //public enum PartyCategory
    //{
    //    Default = 0,
    //    Consumer = 1,
    //    Producer = 2,
    //    Manufacturer = 3,
    //    Retailer = 4,
    //    Wholesaler = 5,
    //    Agent = 6,
    //    Broker = 7
    //}

    public enum ProductType
    {
        Goods = 1,
        Services = 2
    }

    public enum ProductTaxPreference
    {
        Taxable = 1,
        NonTaxable = 2
    }

    public enum InvoiceType
    {
        Stock = 1,
        Purchase = 2,
        PurchaseReturn = 3, //DebitNote
        Sales = 4,
        SalesReturn = 5,    //CreditNote
        Exchange = 6,
        Transfer = 7,
    }

    //Billing Category : It is used to differntiate billing documents based on the requirements for invoice printing...etc Down payment, Billing request etc...we use this billing category = p when we use billing document for down payment.
    //public enum InvoiceCategory
    //{
    //    Cash = 0,
    //    Credit = 1,
    //}

    public enum StockInvoiceType
    {
        None = 0,
        Opening = 1,
        Transfer = 2,
        Wastage = 3,
        Adjustment = 4,
        Loss = 5,
        WriteOff = 6,
        Expired = 7
    }

    public enum GstTreatment
    {
        RegisteredBusiness = 1
    }

    public enum InvoiceTaxPreference
    {
        Taxable = 1,
        TaxExempt = 2
    }

    public enum Status
    {
        Initiated = 0,
        Created = 1,
        Modified = 2,
        Deleted = 3,
        Printed = 4,
        Approved = 5,
        Rejected = 6,
        Cancelled = 7,
        Hold = 8,
        Activated = 9,
        Deactivated = 10
    }

    public enum RoundOffOption
    {
        NoRoundOff = 0,
        NearestTo1 = 1,
        NearestTo5 = 2,
        NearestTo10 = 3,
        NearestTo50 = 4,
        NearestTo100 = 5,
        //Nearest500=5,
        //Nearest1000=6
    }
}