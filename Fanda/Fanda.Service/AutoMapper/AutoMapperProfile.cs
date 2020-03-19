using AutoMapper;
using Fanda.Data.Models;
using Fanda.Dto;
using Fanda.Dto.ViewModels;
using Fanda.Shared.Enums;
using System;
using System.Linq;

namespace Fanda.Service.AutoMapperProfile
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, RegisterViewModel>()
                .ForMember(vm => vm.Password, opt => opt.Ignore())
                //.ForMember(vm => vm.ConfirmPassword, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<Role, RoleDto>()
                .ForMember(vm => vm.RoleId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(x => x.Id, opt => opt.MapFrom(vm => vm.RoleId));
            CreateMap<User, UserDto>()
                .ForMember(vm => vm.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(vm => vm.Token, opt => opt.Ignore())
                //.ForMember(vm => vm.Password, opt => opt.Ignore())
                //.ForPath(vm => vm.Roles, opt => opt.MapFrom(src => src.Roles.Select(c => c.).ToList()))
                .ReverseMap()
                .ForMember(x => x.Id, opt => opt.MapFrom(vm => vm.UserId));

            CreateMap<Location, LocationDto>()
                .ReverseMap();
            CreateMap<Contact, ContactDto>()
                .ForMember(vm => vm.IsDeleted, opt => opt.Ignore())
                .ForMember(vm => vm.Index, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<Address, AddressDto>()
                .ForMember(vm => vm.IsDeleted, opt => opt.Ignore())
                .ForMember(vm => vm.Index, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<PartyCategory, PartyCategoryDto>()
                .ReverseMap();
            CreateMap<ProductCategory, ProductCategoryDto>()
                .ReverseMap();
            CreateMap<Unit, UnitDto>()
                .ReverseMap();
            CreateMap<ProductBrand, ProductBrandDto>()
                .ReverseMap();
            CreateMap<ProductSegment, ProductSegmentDto>()
                .ReverseMap();
            CreateMap<ProductVariety, ProductVarietyDto>()
                .ReverseMap();
            CreateMap<ProductIngredient, ProductIngredientDto>()
                .ReverseMap();
            CreateMap<ProductPricing, ProductPricingDto>()
                .ReverseMap();
            CreateMap<ProductPricingRange, ProductPricingRangeDto>()
                .ReverseMap();
            CreateMap<Product, ProductDto>()
                .ForMember(vm => vm.IsCompoundProduct, opt => opt.MapFrom(src => src.ParentIngredients.Any()))
                .ForMember(vm => vm.Ingredients, opt => opt.MapFrom(src => src.ParentIngredients))
                .ForMember(vm => vm.ProductPricings, opt => opt.MapFrom(src => src.ProductPricings))
                .ReverseMap();

            CreateMap<BankAccount, BankAccountDto>()
                .ForMember(dest => dest.Owner, opt => opt.Ignore())
                .ForMember(dest => dest.OwnerId, opt => opt.Ignore())
                .ForMember(vm => vm.IsDeleted, opt => opt.Ignore())
                .ForMember(vm => vm.Index, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    //Guid partyId = Guid.Empty;
                    if (src.PartyBanks != null && src.PartyBanks.Any())
                    {
                        var partyId = src.PartyBanks.FirstOrDefault().PartyId;
                        if (partyId != null && partyId != Guid.Empty)
                        {
                            dest.OwnerId = partyId;
                            dest.Owner = AccountOwner.Party;
                        }
                    }
                    else if (src.OrgBanks != null && src.OrgBanks.Any())
                    {
                        var orgId = src.OrgBanks.FirstOrDefault().OrgId;
                        if (orgId != null && orgId != Guid.Empty)
                        {
                            dest.OwnerId = orgId;
                            dest.Owner = AccountOwner.Organization;
                        }
                    }
                })
                .ReverseMap()
                .ForMember(x => x.AddressId, opt => opt.MapFrom(vm => vm.Address.AddressId))
                .ForMember(x => x.ContactId, opt => opt.MapFrom(vm => vm.Contact.ContactId));

            CreateMap<Organization, OrganizationDto>()
                .ForPath(vm => vm.Contacts, opt => opt.MapFrom(src => src.Contacts.Select(c => c.Contact).ToList()))
                .ForPath(s => s.Addresses, opt => opt.MapFrom(src => src.Addresses.Select(a => a.Address).ToList()))
                .ForPath(vm => vm.Banks, opt => opt.MapFrom(src => src.Banks.Select(b => b.BankAccount).ToList()))
                .ReverseMap()
                .ForMember(x => x.Contacts,
                    src => src.MapFrom((orgVM, org, i, context) =>
                      {
                          return orgVM.Contacts?.Select(c => new OrgContact
                          {
                              OrgId = new Guid(orgVM.OrgId),
                              Organization = org,
                              ContactId = c.ContactId,
                              Contact = context.Mapper.Map<ContactDto, Contact>(c)
                          }).ToList();
                      }))
                .ForMember(x => x.Addresses,
                    src => src.MapFrom((orgVM, org, i, context) =>
                    {
                        return orgVM.Addresses?.Select(a => new OrgAddress
                        {
                            OrgId = new Guid(orgVM.OrgId),
                            Organization = org,
                            AddressId = a.AddressId,
                            Address = context.Mapper.Map<AddressDto, Address>(a)
                        }).ToList();
                    }))
                .ForMember(x => x.Banks,
                    src => src.MapFrom((orgVM, org, i, context) =>
                    {
                        return orgVM.Banks?.Select(b => new OrgBank
                        {
                            OrgId = new Guid(orgVM.OrgId),
                            Organization = org,
                            BankAcctId = b.BankAcctId,
                            BankAccount = context.Mapper.Map<BankAccountDto, BankAccount>(b)
                        }).ToList();
                    }));

            CreateMap<Party, PartyDto>()
                .ForMember(vm => vm.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForPath(vm => vm.Contacts, m => m.MapFrom(s => s.Contacts.Select(pc => pc.Contact).ToList()))
                .ForPath(vm => vm.Addresses, m => m.MapFrom(s => s.Addresses.Select(pa => pa.Address).ToList()))
                .ForPath(vm => vm.Banks, m => m.MapFrom(s => s.Banks.Select(pb => pb.BankAccount).ToList()))
                .ReverseMap()
                .ForMember(x => x.Category, opt => opt.Ignore())
                .ForMember(x => x.Contacts,
                    src => src.MapFrom((partyVM, party, i, context) =>
                    {
                        return partyVM.Contacts?.Select(c => new PartyContact
                        {
                            PartyId = new Guid(partyVM.PartyId),
                            Party = party,
                            ContactId = c.ContactId,
                            Contact = context.Mapper.Map<ContactDto, Contact>(c)
                        }).ToList();
                    }))
                .ForMember(x => x.Addresses,
                    src => src.MapFrom((partyVM, party, i, context) =>
                    {
                        return partyVM.Addresses?.Select(a => new PartyAddress
                        {
                            PartyId = new Guid(partyVM.PartyId),
                            Party = party,
                            AddressId = a.AddressId,
                            Address = context.Mapper.Map<AddressDto, Address>(a)
                        }).ToList();
                    }))
                .ForMember(x => x.Banks,
                    src => src.MapFrom((partyVM, party, i, context) =>
                    {
                        return partyVM.Banks?.Select(b => new PartyBank
                        {
                            PartyId = new Guid(partyVM.PartyId),
                            Party = party,
                            BankAcctId = b.BankAcctId,
                            BankAccount = context.Mapper.Map<BankAccountDto, BankAccount>(b)
                        }).ToList();
                    }));

            CreateMap<InvoiceCategory, InvoiceCategoryDto>()
                .ReverseMap();

            CreateMap<Stock, StockDto>()
                .ReverseMap();
            CreateMap<InvoiceItem, InvoiceItemDto>()
                .ReverseMap();
            CreateMap<Invoice, InvoiceDto>()
                .ReverseMap();
        }
    }

    //public class BankViewModelConverter : ITypeConverter<BankAccount, BankAccountViewModel>
    //{
    //    public BankAccountViewModel Convert(BankAccount source, BankAccountViewModel destination, ResolutionContext context)
    //    {
    //        return new BankAccountViewModel
    //        {
    //            AccountId = source.Id,
    //            Owner = source.PartyBanks.Any() ? Common.Enums.Owner.Party :
    //                (source.OrgBanks.Any() ? Common.Enums.Owner.Organization : Common.Enums.Owner.None),
    //            OwnerId = source.PartyBanks.Any() ? source.PartyBanks.FirstOrDefault()?.PartyId :
    //                (source.OrgBanks.Any() ? source.OrgBanks.FirstOrDefault()?.OrgId : Guid.Empty)
    //        };
    //    }
    //}
}