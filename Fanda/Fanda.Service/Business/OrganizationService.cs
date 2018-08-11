using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Common.Utility;
using Fanda.Data.Business;
using Fanda.Data.Context;
using Fanda.ViewModel.Base;
using Fanda.ViewModel.Business;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanda.Service.Business
{
    public interface IOrganizationService
    {
        Task<List<OrganizationViewModel>> GetAllAsync(bool? active);

        Task<OrganizationViewModel> GetByIdAsync(Guid orgId);

        Task<OrganizationViewModel> SaveAsync(OrganizationViewModel org);

        Task<bool> DeleteAsync(Guid orgId);

        string ErrorMessage { get; }
    }

    public class OrganizationService : IOrganizationService
    {
        private FandaContext _context;
        private IMapper _mapper;

        public OrganizationService(FandaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string ErrorMessage { get; private set; }

        public async Task<List<OrganizationViewModel>> GetAllAsync(bool? active)
        {
            try
            {
                var orgs = await _context.Organizations
                    .Where(p => p.Active == ((active == null) ? p.Active : active))
                    .AsNoTracking()
                    //.ProjectTo<OrganizationViewModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                //return new PagedList<OrganizationViewModel>(orgs, pagingParams);
                return _mapper.Map<List<OrganizationViewModel>>(orgs);
                //return _mapper.Map<IEnumerable<OrganizationViewModel>>(await _context.Organizations.ToListAsync());
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerMessage();
                return null;
            }
        }

        public async Task<OrganizationViewModel> GetByIdAsync(Guid orgId)
        {
            try
            {
                if (orgId == null || orgId == Guid.Empty)
                    throw new ArgumentNullException("orgId", "Org id is missing");

                var org = await _context.Organizations
                    .ProjectTo<OrganizationViewModel>(_mapper.ConfigurationProvider)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(o => o.OrgId == orgId);

                if (org != null)
                {
                    org.Contacts = await _context.Organizations
                        .Where(m => m.OrgId == orgId)
                        .SelectMany(oc => oc.Contacts.Select(c => c.Contact))
                        .ProjectTo<ContactViewModel>(_mapper.ConfigurationProvider)
                        .AsNoTracking()
                        .ToListAsync();
                    org.Addresses = await _context.Organizations
                        .Where(m => m.OrgId == orgId)
                        .SelectMany(oa => oa.Addresses.Select(a => a.Address))
                        .ProjectTo<AddressViewModel>(_mapper.ConfigurationProvider)
                        .AsNoTracking()
                        .ToListAsync();

                    return org;
                }

                throw new KeyNotFoundException("Organization not found");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerMessage();
                return null;
            }
        }

        public async Task<OrganizationViewModel> SaveAsync(OrganizationViewModel orgVM)
        {
            try
            {
                var org = _mapper.Map<Organization>(orgVM);
                if (org.OrgId == Guid.Empty)
                {
                    org.DateCreated = DateTime.Now;
                    org.DateModified = null;
                    _context.Organizations.Add(org);
                }
                else
                {
                    var dbOrg = await _context.Organizations
                        .Where(o => o.OrgId == org.OrgId)
                        .Include(o => o.Contacts).ThenInclude(oc => oc.Contact)
                        .Include(o => o.Addresses).ThenInclude(oa => oa.Address)
                        .SingleOrDefaultAsync();
                    if (dbOrg == null)
                    {
                        org.DateCreated = DateTime.Now;
                        org.DateModified = null;
                        _context.Organizations.Add(org);
                    }
                    else
                    {
                        // delete all contacts that no longer exists
                        foreach (var dbOrgContact in dbOrg.Contacts)
                        {
                            var dbContact = dbOrgContact.Contact;
                            if (org.Contacts.All(oc => oc.Contact.ContactId != dbContact.ContactId))
                                _context.Contacts.Remove(dbContact);
                        }
                        // delete all addresses that no longer exists
                        foreach (var dbOrgAddress in dbOrg.Addresses)
                        {
                            var dbAddress = dbOrgAddress.Address;
                            if (org.Addresses.All(oa => oa.Address.AddressId != dbAddress.AddressId))
                                _context.Addresses.Remove(dbAddress);
                        }
                        // copy current (incoming) values to db
                        org.DateModified = DateTime.Now;
                        _context.Entry(dbOrg).CurrentValues.SetValues(org);
                        //_context.Entry(dbOrg).CurrentValues["DateModified"] = DateTime.Now;
                        var contactPairs = from curr in org.Contacts.Select(oc => oc.Contact)
                                           join db in dbOrg.Contacts.Select(oc => oc.Contact)
                                             on curr.ContactId equals db.ContactId into grp
                                           from db in grp.DefaultIfEmpty()
                                           select new { curr, db };
                        foreach (var pair in contactPairs)
                        {
                            if (pair.db != null)
                                _context.Entry(pair.db).CurrentValues.SetValues(pair.curr);
                            else
                            {
                                //_context.Set<Contact>().Add(pair.curr);
                                _context.Set<OrgContact>().Add(new OrgContact
                                {
                                    OrgId = org.OrgId,
                                    Organization = org,
                                    ContactId = pair.curr.ContactId,
                                    Contact = pair.curr
                                });
                            }
                        }
                        var addressPairs = from curr in org.Addresses.Select(oa => oa.Address)
                                           join db in dbOrg.Addresses.Select(oa => oa.Address)
                                             on curr.AddressId equals db.AddressId into grp
                                           from db in grp.DefaultIfEmpty()
                                           select new { curr, db };
                        foreach (var pair in addressPairs)
                        {
                            if (pair.db != null)
                                _context.Entry(pair.db).CurrentValues.SetValues(pair.curr);
                            else
                            {
                                //_context.Set<Address>().Add(pair.curr);
                                _context.Set<OrgAddress>().Add(new OrgAddress
                                {
                                    OrgId = org.OrgId,
                                    Organization = org,
                                    AddressId = pair.curr.AddressId,
                                    Address = pair.curr
                                });
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
                orgVM = _mapper.Map<OrganizationViewModel>(org);
                return orgVM;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerMessage();
                return null;
            }
        }

        public async Task<bool> DeleteAsync(Guid orgId)
        {
            try
            {
                var org = await _context.Organizations
                    .Include(o => o.Contacts).ThenInclude(oc => oc.Contact)
                    .Include(o => o.Addresses).ThenInclude(oa => oa.Address)
                    .SingleOrDefaultAsync(o => o.OrgId == orgId);
                if (org != null)
                {
                    foreach (var orgContact in org.Contacts)
                        _context.Contacts.Remove(orgContact.Contact);
                    foreach (var orgAddress in org.Addresses)
                        _context.Addresses.Remove(orgAddress.Address);
                    _context.Organizations.Remove(org);
                    await _context.SaveChangesAsync();
                    return true;
                }
                throw new KeyNotFoundException("Organization not found");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerMessage();
                return false;
            }
        }
    }
}