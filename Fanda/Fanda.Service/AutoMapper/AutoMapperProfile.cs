﻿using AutoMapper;
using Fanda.Data.Access;
using Fanda.Data.Base;
using Fanda.Data.Business;
using Fanda.Data.Commodity;
using Fanda.Data.Inventory;
using Fanda.ViewModel.Access;
using Fanda.ViewModel.Base;
using Fanda.ViewModel.Business;
using Fanda.ViewModel.Commodity;
using Fanda.ViewModel.Inventory;
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
                .ForMember(vm => vm.ConfirmPassword, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<Role, RoleViewModel>()
                .ForMember(vm => vm.RoleId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(x => x.Id, opt => opt.MapFrom(vm => vm.RoleId));
            CreateMap<User, UserViewModel>()
                .ForMember(vm => vm.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(vm => vm.Password, opt => opt.Ignore())
                //.ForPath(vm => vm.Roles, opt => opt.MapFrom(src => src.Roles.Select(c => c.).ToList()))
                .ReverseMap()
                .ForMember(x => x.Id, opt => opt.MapFrom(vm => vm.UserId));

            CreateMap<Contact, ContactViewModel>()
                .ReverseMap();
            CreateMap<Address, AddressViewModel>()
                .ReverseMap();
            CreateMap<PartyCategory, PartyCategoryViewModel>()
                .ReverseMap();
            CreateMap<ProductCategory, ProductCategoryViewModel>()
                .ReverseMap();
            CreateMap<Unit, UnitViewModel>()
                .ReverseMap();
            CreateMap<ProductBrand, ProductBrandViewModel>()
                .ReverseMap();
            CreateMap<ProductSegment, ProductSegmentViewModel>()
                .ReverseMap();
            CreateMap<ProductVariety, ProductVarietyViewModel>()
                .ReverseMap();
            CreateMap<ProductIngredient, ProductIngredientViewModel>()
                .ReverseMap();
            CreateMap<ProductPricing, ProductPricingViewModel>()
                .ReverseMap();
            CreateMap<ProductPricingRange, ProductPricingRangeViewModel>()
                .ReverseMap();
            CreateMap<Product, ProductViewModel>()
                .ForMember(vm => vm.IsCompoundProduct, opt => opt.MapFrom(src => src.ParentIngredients.Any()))
                .ForMember(vm => vm.Ingredients, opt => opt.MapFrom(src => src.ParentIngredients))
                .ForMember(vm => vm.ProductPricings, opt => opt.MapFrom(src => src.ProductPricings))
                .ReverseMap();

            CreateMap<BankAccount, BankAccountViewModel>()
                //.ForMember(vm => vm.Contact, m => m.MapFrom(s => s.Contacts.FirstOrDefault().Contact))
                //.ForMember(vm => vm.Address, m => m.MapFrom(s => s.Addresses.FirstOrDefault().Address))
                .ForMember(dest => dest.Owner, opt => opt.Ignore())
                .ForMember(dest => dest.OwnerId, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    //Guid partyId = Guid.Empty;
                    if (src.PartyBanks != null && src.PartyBanks.Any())
                    {
                        var partyId = src.PartyBanks.FirstOrDefault().PartyId;
                        if (partyId != null && partyId != Guid.Empty)
                        {
                            dest.OwnerId = partyId;
                            dest.Owner = Common.Enums.AccountOwner.Party;
                        }
                    }
                    else if (src.OrgBanks != null && src.OrgBanks.Any())
                    {
                        var orgId = src.OrgBanks.FirstOrDefault().OrgId;
                        if (orgId != null && orgId != Guid.Empty)
                        {
                            dest.OwnerId = orgId;
                            dest.Owner = Common.Enums.AccountOwner.Organization;
                        }
                    }
                })
                .ReverseMap()
                .ForMember(x => x.AddressId, opt => opt.MapFrom(vm => vm.Address.AddressId))
                .ForMember(x => x.ContactId, opt => opt.MapFrom(vm => vm.Contact.ContactId))
                //.ForMember(x => x.Contacts, opt => opt.MapFrom(vm => new[] { vm.Contact }))
                //.ForMember(x => x.Addresses, opt => opt.MapFrom(vm => new[] { vm.Address }))
                ;

            CreateMap<Organization, OrganizationViewModel>()
                .ForPath(vm => vm.Contacts, opt => opt.MapFrom(src => src.Contacts.Select(c => c.Contact).ToList()))
                .ForPath(s => s.Addresses, opt => opt.MapFrom(src => src.Addresses.Select(a => a.Address).ToList()))
                .ReverseMap()
                .ForMember(x => x.Contacts,
                    src => src.ResolveUsing((orgVM, org, i, context) =>
                      {
                          return orgVM.Contacts?.Select(c => new OrgContact
                          {
                              OrgId = orgVM.OrgId,
                              Organization = org,
                              ContactId = c.ContactId,
                              Contact = context.Mapper.Map<ContactViewModel, Contact>(c)
                          }).ToList();
                      }))
                .ForMember(x => x.Addresses,
                    src => src.ResolveUsing((orgVM, org, i, context) =>
                    {
                        return orgVM.Addresses?.Select(a => new OrgAddress
                        {
                            OrgId = orgVM.OrgId,
                            Organization = org,
                            AddressId = a.AddressId,
                            Address = context.Mapper.Map<AddressViewModel, Address>(a)
                        }).ToList();
                    }))
                ;

            CreateMap<Party, PartyViewModel>()
                .ForPath(vm => vm.Contacts, m => m.MapFrom(s => s.Contacts.Select(pc => pc.Contact).ToList()))
                .ForPath(vm => vm.Addresses, m => m.MapFrom(s => s.Addresses.Select(pa => pa.Address).ToList()))
                .ReverseMap()
                .ForMember(x => x.Contacts,
                    src => src.ResolveUsing((partyVM, party, i, context) =>
                    {
                        return partyVM.Contacts?.Select(c => new PartyContact
                        {
                            PartyId = partyVM.PartyId,
                            Party = party,
                            ContactId = c.ContactId,
                            Contact = context.Mapper.Map<ContactViewModel, Contact>(c)
                        }).ToList();
                    }))
                .ForMember(x => x.Addresses,
                    src => src.ResolveUsing((partyVM, party, i, context) =>
                    {
                        return partyVM.Addresses?.Select(a => new PartyAddress
                        {
                            PartyId = partyVM.PartyId,
                            Party = party,
                            AddressId = a.AddressId,
                            Address = context.Mapper.Map<AddressViewModel, Address>(a)
                        }).ToList();
                    }));

            CreateMap<InvoiceCategory, InvoiceCategoryViewModel>()
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