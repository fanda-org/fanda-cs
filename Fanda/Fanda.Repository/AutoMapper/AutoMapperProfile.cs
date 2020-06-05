﻿using AutoMapper;
using Fanda.Dto;
using Fanda.Dto.ViewModels;
using Fanda.Models;
using System.Linq;

namespace Fanda.Repository.AutoMapperProfile
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, RegisterViewModel>()
                .ForMember(vm => vm.Username, opt => opt.MapFrom(src => src.Name))
                .ForMember(vm => vm.Password, opt => opt.Ignore())
                .ForMember(vm => vm.ConfirmPassword, opt => opt.Ignore())
                .ForMember(vm => vm.AgreeTerms, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<RefreshToken, RefreshTokenDto>()
                .ReverseMap();
            CreateMap<RefreshToken, ActiveTokenDto>();
            CreateMap<Role, RoleDto>()
                .ForMember(vm => vm.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(x => x.Id, opt => opt.MapFrom(vm => vm.Id));
            CreateMap<User, UserDto>()
                .ForMember(vm => vm.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(vm => vm.Token, opt => opt.Ignore())
                //.ForPath(vm => vm.Organizations, opt => opt.MapFrom(src => src.OrgUsers.Select(c => c.Organization).ToList()))
                .ForMember(vm => vm.Password, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(x => x.Id, opt => opt.MapFrom(vm => vm.Id));

            //CreateMap<Location, LocationDto>()
            //    .ReverseMap();
            CreateMap<Contact, ContactDto>()
                .ForMember(vm => vm.IsDeleted, opt => opt.Ignore())
                .ForMember(vm => vm.Index, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<Address, AddressDto>()
                //.ForMember(vm => vm.IsDeleted, opt => opt.Ignore())
                //.ForMember(vm => vm.Index, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<PartyCategory, PartyCategoryDto>()
                .ReverseMap();
            CreateMap<ProductCategory, ProductCategoryDto>()
                .ReverseMap();
            CreateMap<Unit, UnitDto>()
                .ReverseMap();
            CreateMap<UnitConversion, UnitConversionDto>()
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

            CreateMap<Bank, BankDto>()
                //.ForMember(dest => dest.Owner, opt => opt.Ignore())
                //.ForMember(dest => dest.OwnerId, opt => opt.Ignore())
                //.ForMember(vm => vm.IsDeleted, opt => opt.Ignore())
                //.ForMember(vm => vm.Index, opt => opt.Ignore())
                //.AfterMap((src, dest) =>
                //{
                //    //Guid partyId = Guid.Empty;
                //    if (src.PartyBanks != null && src.PartyBanks.Any())
                //    {
                //        var partyId = src.PartyBanks.FirstOrDefault().PartyId;
                //        if (partyId != null && partyId != Guid.Empty)
                //        {
                //            dest.OwnerId = partyId;
                //            dest.Owner = AccountOwner.Party;
                //        }
                //    }
                //    else if (src.OrgBanks != null && src.OrgBanks.Any())
                //    {
                //        var orgId = src.OrgBanks.FirstOrDefault().OrgId;
                //        if (orgId != null && orgId != Guid.Empty)
                //        {
                //            dest.OwnerId = orgId;
                //            dest.Owner = AccountOwner.Organization;
                //        }
                //    }
                //})
                .ReverseMap()
                .ForMember(x => x.AddressId, opt => opt.MapFrom(vm => vm.Address.Id))
                .ForMember(x => x.ContactId, opt => opt.MapFrom(vm => vm.Contact.Id));

            CreateMap<Organization, OrganizationDto>()
                //.ForPath(vm => vm.Users, opt => opt.MapFrom(src => src.OrgUsers.Select(ou => ou.User).ToList()))
                .ForPath(vm => vm.Contacts, opt => opt.MapFrom(src => src.OrgContacts.Select(c => c.Contact).ToList()))
                .ForPath(s => s.Addresses, opt => opt.MapFrom(src => src.OrgAddresses.Select(a => a.Address).ToList()))
                //.ForPath(vm => vm.Banks, opt => opt.MapFrom(src => src.Banks.Select(b => b.BankAccount).ToList()))
                .ReverseMap()
                .ForMember(x => x.OrgContacts,
                    src => src.MapFrom((orgVM, org, oc, context) =>
                      {
                          return orgVM.Contacts?.Select(c => new OrgContact
                          {
                              OrgId = orgVM.Id,
                              Organization = org,
                              ContactId = c.Id,
                              Contact = context.Mapper.Map<ContactDto, Contact>(c)
                          }).ToList();
                      }))
                .ForMember(x => x.OrgAddresses,
                    src => src.MapFrom((orgVM, org, oa, context) =>
                    {
                        return orgVM.Addresses?.Select(a => new OrgAddress
                        {
                            OrgId = orgVM.Id,
                            Organization = org,
                            AddressId = a.Id,
                            Address = context.Mapper.Map<AddressDto, Address>(a)
                        }).ToList();
                    }));
            //.ForMember(x => x.Banks,
            //    src => src.MapFrom((orgVM, org, i, context) =>
            //    {
            //        return orgVM.Banks?.Select(b => new OrgBank
            //        {
            //            OrgId = new Guid(orgVM.OrgId),
            //            Organization = org,
            //            BankAcctId = b.BankAcctId,
            //            BankAccount = context.Mapper.Map<BankDto, Bank>(b)
            //        }).ToList();
            //    }));

            CreateMap<Party, PartyDto>()
                .ForMember(vm => vm.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForPath(vm => vm.Contacts, m => m.MapFrom(s => s.PartyContacts.Select(pc => pc.Contact).ToList()))
                .ForPath(vm => vm.Addresses, m => m.MapFrom(s => s.PartyAddresses.Select(pa => pa.Address).ToList()))
                //.ForPath(vm => vm.Banks, m => m.MapFrom(s => s.Banks.Select(pb => pb.BankAccount).ToList()))
                .ReverseMap()
                .ForMember(x => x.Category, opt => opt.Ignore())
                .ForMember(x => x.PartyContacts,
                    src => src.MapFrom((partyVM, party, i, context) =>
                    {
                        return partyVM.Contacts?.Select(c => new PartyContact
                        {
                            PartyId = partyVM.LedgerId,
                            Party = party,
                            ContactId = c.Id,
                            Contact = context.Mapper.Map<ContactDto, Contact>(c)
                        }).ToList();
                    }))
                .ForMember(x => x.PartyAddresses,
                    src => src.MapFrom((partyVM, party, i, context) =>
                    {
                        return partyVM.Addresses?.Select(a => new PartyAddress
                        {
                            PartyId = partyVM.LedgerId,
                            Party = party,
                            AddressId = a.Id,
                            Address = context.Mapper.Map<AddressDto, Address>(a)
                        }).ToList();
                    }));
            //.ForMember(x => x.Banks,
            //    src => src.MapFrom((partyVM, party, i, context) =>
            //    {
            //        return partyVM.Banks?.Select(b => new PartyBank
            //        {
            //            PartyId = new Guid(partyVM.PartyId),
            //            Party = party,
            //            BankAcctId = b.BankAcctId,
            //            BankAccount = context.Mapper.Map<BankDto, Bank>(b)
            //        }).ToList();
            //    }));

            CreateMap<InvoiceCategory, InvoiceCategoryDto>()
                .ReverseMap();

            CreateMap<Stock, StockDto>()
                .ReverseMap();
            CreateMap<InvoiceItem, InvoiceItemDto>()
                .ReverseMap();
            CreateMap<Invoice, InvoiceDto>()
                .ReverseMap();

            CreateMap<AccountYear, AccountYearDto>()
                .ReverseMap();

            #region ViewModel to Dto
            CreateMap<UserDto, RegisterViewModel>()
                .ForMember(vm => vm.Username, opt => opt.MapFrom(src => src.Name))
                .ForMember(vm => vm.ConfirmPassword, opt => opt.Ignore())
                .ForMember(vm => vm.AgreeTerms, opt => opt.Ignore())
                .ReverseMap();
            #endregion

            #region List mappings
            CreateMap<AccountYear, YearListDto>();
            //.ReverseMap();
            CreateMap<Organization, OrgListDto>();
            //.ReverseMap();
            CreateMap<Organization, OrgYearListDto>()
                .ForMember(vm => vm.SelectedYearId, opt => opt.Ignore())
                .ForMember(vm => vm.IsSelected, opt => opt.Ignore());
            //.ReverseMap();
            CreateMap<PartyCategory, PartyCategoryListDto>();
            //.ReverseMap();
            CreateMap<ProductCategory, ProductCategoryListDto>();
            //.ReverseMap();
            CreateMap<Unit, UnitListDto>();
            //.ReverseMap();
            CreateMap<ProductBrand, ProductBrandListDto>();
            //.ReverseMap();
            CreateMap<User, UserListDto>();
            //.ForPath(vm => vm.OrgId,
            //    opt => opt.MapFrom(src => src.OrgUsers.Select(ou => ou.OrgId).First()));
            #endregion
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