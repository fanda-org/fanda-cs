using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Data;
using Fanda.Data.Context;
using Fanda.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanda.Service
{
    public interface IOrganizationService
    {
        IQueryable<OrganizationDto> GetAll();
        Task<OrganizationDto> GetByIdAsync(Guid orgId, bool includes = false);
        //Task<OrganizationDto> GetByCodeAsync(Guid orgCode, bool includes = false);
        Task<OrganizationDto> SaveAsync(OrganizationDto dto);
        Task<bool> DeleteAsync(Guid orgId);
        bool ExistsById(Guid orgId);
        bool ExistsByCode(string orgCode);
        Task<bool> ChangeStatus(Guid orgId, bool active);
        string ErrorMessage { get; }
    }

    public class OrganizationService : IOrganizationService
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;

        public OrganizationService(FandaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string ErrorMessage { get; private set; }

        public IQueryable<OrganizationDto> GetAll()
        {
            var orgs = _context.Organizations
                .AsNoTracking()
                .ProjectTo<OrganizationDto>(_mapper.ConfigurationProvider);
            return orgs;
        }

        public async Task<OrganizationDto> GetByIdAsync(Guid orgId, bool includes = false)
        {
            if (orgId == null || orgId == Guid.Empty)
                throw new ArgumentNullException("orgId", "Org id is missing");

            var org = await _context.Organizations
                .AsNoTracking()
                .ProjectTo<OrganizationDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(o => o.Id == orgId);

            if (org == null)
                throw new KeyNotFoundException("Organization not found");
            if (org != null && !includes)
                return org;

            org.Contacts = await _context.Organizations
                .AsNoTracking()
                .Where(m => m.Id == orgId)
                .SelectMany(oc => oc.OrgContacts.Select(c => c.Contact))
                .ProjectTo<ContactDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            org.Addresses = await _context.Organizations
                .AsNoTracking()
                .Where(m => m.Id == orgId)
                .SelectMany(oa => oa.OrgAddresses.Select(a => a.Address))
                .ProjectTo<AddressDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return org;
        }

        //public async Task<OrganizationDto> GetByCodeAsync(string orgCode, bool includes = false)
        //{
        //    if (string.IsNullOrEmpty(orgCode))
        //        throw new ArgumentNullException("orgCode", "Org code is missing");

        //    var org = await _context.Organizations
        //        .AsNoTracking()
        //        .ProjectTo<OrganizationDto>(_mapper.ConfigurationProvider)
        //        .FirstOrDefaultAsync(o => o.OrgCode == orgCode);

        //    if (org == null)
        //        throw new KeyNotFoundException("Organization not found");
        //    if (org != null && !includes)
        //        return org;

        //    Guid guid = new Guid(orgCode);
        //    org.Contacts = await _context.Organizations
        //        .AsNoTracking()
        //        .Where(m => m.Id == guid)
        //        .SelectMany(oc => oc.OrgContacts.Select(c => c.Contact))
        //        .ProjectTo<ContactDto>(_mapper.ConfigurationProvider)
        //        .ToListAsync();
        //    org.Addresses = await _context.Organizations
        //        .AsNoTracking()
        //        .Where(m => m.Id == guid)
        //        .SelectMany(oa => oa.OrgAddresses.Select(a => a.Address))
        //        .ProjectTo<AddressDto>(_mapper.ConfigurationProvider)
        //        .ToListAsync();
        //    return org;

        //}

        public async Task<OrganizationDto> SaveAsync(OrganizationDto orgVM)
        {
            var org = _mapper.Map<Organization>(orgVM);
            if (org.Id == Guid.Empty)
            {
                org.DateCreated = DateTime.Now;
                org.DateModified = null;
                await _context.Organizations.AddAsync(org);
            }
            else
            {
                var dbOrg = await _context.Organizations
                    .Where(o => o.Id == org.Id)
                    .Include(o => o.OrgContacts).ThenInclude(oc => oc.Contact)
                    .Include(o => o.OrgAddresses).ThenInclude(oa => oa.Address)
                    .FirstOrDefaultAsync();
                if (dbOrg == null)
                {
                    org.DateCreated = DateTime.Now;
                    org.DateModified = null;
                    await _context.Organizations.AddAsync(org);
                }
                else
                {
                    // delete all contacts that no longer exists
                    foreach (var dbOrgContact in dbOrg.OrgContacts)
                    {
                        var dbContact = dbOrgContact.Contact;
                        if (org.OrgContacts.All(oc => oc.Contact.Id != dbContact.Id))
                            _context.Contacts.Remove(dbContact);
                    }
                    // delete all addresses that no longer exists
                    foreach (var dbOrgAddress in dbOrg.OrgAddresses)
                    {
                        var dbAddress = dbOrgAddress.Address;
                        if (org.OrgAddresses.All(oa => oa.Address.Id != dbAddress.Id))
                            _context.Addresses.Remove(dbAddress);
                    }
                    // copy current (incoming) values to db
                    org.DateModified = DateTime.Now;
                    _context.Entry(dbOrg).CurrentValues.SetValues(org);

                    #region Contacts
                    var contactPairs = from curr in org.OrgContacts.Select(oc => oc.Contact)
                                       join db in dbOrg.OrgContacts.Select(oc => oc.Contact)
                                         on curr.Id equals db.Id into grp
                                       from db in grp.DefaultIfEmpty()
                                       select new { curr, db };
                    foreach (var pair in contactPairs)
                    {
                        if (pair.db != null)
                            _context.Entry(pair.db).CurrentValues.SetValues(pair.curr);
                        else
                        {
                            //_context.Set<Contact>().Add(pair.curr);
                            await _context.Set<OrgContact>().AddAsync(new OrgContact
                            {
                                OrgId = org.Id,
                                Organization = org,
                                ContactId = pair.curr.Id,
                                Contact = pair.curr
                            });
                        }
                    }
                    #endregion

                    #region Addresses
                    var addressPairs = from curr in org.OrgAddresses.Select(oa => oa.Address)
                                       join db in dbOrg.OrgAddresses.Select(oa => oa.Address)
                                         on curr.Id equals db.Id into grp
                                       from db in grp.DefaultIfEmpty()
                                       select new { curr, db };
                    foreach (var pair in addressPairs)
                    {
                        if (pair.db != null)
                            _context.Entry(pair.db).CurrentValues.SetValues(pair.curr);
                        else
                        {
                            //_context.Set<Address>().Add(pair.curr);
                            await _context.Set<OrgAddress>().AddAsync(new OrgAddress
                            {
                                OrgId = org.Id,
                                Organization = org,
                                AddressId = pair.curr.Id,
                                Address = pair.curr
                            });
                        }
                    }
                    #endregion
                }
            }

            await _context.SaveChangesAsync();
            orgVM = _mapper.Map<OrganizationDto>(org);
            return orgVM;
        }

        public async Task<bool> DeleteAsync(Guid orgId)
        {
            if (orgId == null || orgId == Guid.Empty)
                throw new ArgumentNullException("orgId", "Org id is missing");

            var org = await _context.Organizations
                .Include(o => o.OrgContacts).ThenInclude(oc => oc.Contact)
                .Include(o => o.OrgAddresses).ThenInclude(oa => oa.Address)
                .SingleOrDefaultAsync(o => o.Id == orgId);
            if (org != null)
            {
                foreach (var orgContact in org.OrgContacts)
                    _context.Contacts.Remove(orgContact.Contact);
                foreach (var orgAddress in org.OrgAddresses)
                    _context.Addresses.Remove(orgAddress.Address);
                _context.Organizations.Remove(org);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Organization not found");
        }

        public bool ExistsById(Guid orgId) => _context.Organizations.Any(o => o.Id == orgId);

        public bool ExistsByCode(string orgCode) => _context.Organizations.Any(o => o.OrgCode == orgCode);

        public async Task<bool> ChangeStatus(Guid orgId, bool active)
        {
            if (orgId == null || orgId == Guid.Empty)
                throw new ArgumentNullException("orgId", "Org id is missing");

            var org = await _context.Organizations
                .FindAsync(orgId);
            if (org != null)
            {
                org.Active = active;
                _context.Organizations.Update(org);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Organization not found");
        }
    }
}