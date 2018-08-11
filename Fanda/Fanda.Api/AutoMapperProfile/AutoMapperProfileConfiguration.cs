using AutoMapper;
using Fanda.Data.Entities;
using Fanda.ViewModel.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace Fanda.Api.AutoMapperProfile
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            CreateMap<Role, RoleViewModel>()
                .ForMember(vm => vm.RoleId, opt => opt.MapFrom(src => src.Id))
                .ForMember(vm => vm.RoleCode, opt => opt.MapFrom(src => src.Code))
                .ForMember(vm => vm.RoleName, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();
            CreateMap<User, UserViewModel>()
                .ForMember(vm => vm.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(vm => vm.Password, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(x => x.Id, opt => opt.MapFrom(vm => vm.UserId));
            //CreateMap<OrgUser, UserViewModel>()
            //    .ForAllMembers(o => o.MapFrom(src => src.User));
            //CreateMap<OrgUser, OrganizationViewModel>()
            //    .ForAllMembers(opt => opt.MapFrom(src => src.Organization));

            CreateMap<Contact, ContactViewModel>()
                .ForMember(vm => vm.ContactId, m => m.MapFrom(s => s.Id))
                .ReverseMap();

            CreateMap<Address, AddressViewModel>()
                .ForMember(vm => vm.AddressId, m => m.MapFrom(s => s.Id))
                .ReverseMap();

            CreateMap<BankAccount, BankAccountViewModel>()
                .ForMember(vm => vm.AccountId, m => m.MapFrom(s => s.Id))
                .ForMember(vm => vm.Contact, m => m.MapFrom(s => s.Contacts.FirstOrDefault().Contact))
                .ForMember(vm => vm.Address, m => m.MapFrom(s => s.Addresses.FirstOrDefault().Address))
                .ReverseMap()
                .ForMember(x => x.Contacts, opt => opt.MapFrom(vm => new[] { vm.Contact }))
                .ForMember(x => x.Addresses, opt => opt.MapFrom(vm => new[] { vm.Address }));

            CreateMap<Organization, OrganizationViewModel>()
                .ForMember(vm => vm.OrgId, m => m.MapFrom(s => s.Id))
                .ForPath(vm => vm.Contacts, opt => opt.MapFrom(src => src.Contacts.Select(c => c.Contact).ToList()))
                .ForPath(s => s.Addresses, opt => opt.MapFrom(src => src.Addresses.Select(a => a.Address).ToList()))
                .ReverseMap()
                .ForPath(x => x.Contacts,
                    opt => opt.MapFrom(vm => vm.Contacts.Select(y => new OrgContact { OrgId = vm.OrgId, ContactId = y.ContactId })))
                .ForPath(x => x.Addresses,
                    opt => opt.MapFrom(vm => vm.Addresses.Select(y => new OrgAddress { OrgId = vm.OrgId, AddressId = y.AddressId })));
        }
    }
}
