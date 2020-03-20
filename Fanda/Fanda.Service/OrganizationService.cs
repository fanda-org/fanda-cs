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
        Task<OrganizationDto> GetByIdAsync(string orgId, bool includes = false);
        Task<OrganizationDto> GetByCodeAsync(string orgCode, bool includes = false);
        Task<OrganizationDto> SaveAsync(OrganizationDto orgVM);
        Task<bool> DeleteAsync(string orgId);
        bool Exists(string orgCode);
        Task<bool> ChangeStatus(string orgId, bool active);
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

        public async Task<OrganizationDto> GetByIdAsync(string orgId, bool includes = false)
        {
            if (string.IsNullOrEmpty(orgId))
                throw new ArgumentNullException("orgId", "Org id is missing");

            var org = await _context.Organizations
                .AsNoTracking()
                .ProjectTo<OrganizationDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(o => o.OrgId == orgId);

            if (org == null)
                throw new KeyNotFoundException("Organization not found");
            if (org != null && !includes)
                return org;

            Guid guid = new Guid(orgId);
            org.Contacts = await _context.Organizations
                .AsNoTracking()
                .Where(m => m.OrgId == guid)
                .SelectMany(oc => oc.Contacts.Select(c => c.Contact))
                .ProjectTo<ContactDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            org.Addresses = await _context.Organizations
                .AsNoTracking()
                .Where(m => m.OrgId == guid)
                .SelectMany(oa => oa.Addresses.Select(a => a.Address))
                .ProjectTo<AddressDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            org.Banks = await _context.Organizations
                .AsNoTracking()
                .Where(m => m.OrgId == guid)
                .SelectMany(pb => pb.Banks.Select(a => a.BankAccount))
                .ProjectTo<BankAccountDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return org;
        }

        public async Task<OrganizationDto> GetByCodeAsync(string orgCode, bool includes = false)
        {
            if (string.IsNullOrEmpty(orgCode))
                throw new ArgumentNullException("orgCode", "Org code is missing");

            var org = await _context.Organizations
                .AsNoTracking()
                .ProjectTo<OrganizationDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(o => o.OrgCode == orgCode);

            if (org == null)
                throw new KeyNotFoundException("Organization not found");
            if (org != null && !includes)
                return org;

            Guid guid = new Guid(orgCode);
            org.Contacts = await _context.Organizations
                .AsNoTracking()
                .Where(m => m.OrgId == guid)
                .SelectMany(oc => oc.Contacts.Select(c => c.Contact))
                .ProjectTo<ContactDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            org.Addresses = await _context.Organizations
                .AsNoTracking()
                .Where(m => m.OrgId == guid)
                .SelectMany(oa => oa.Addresses.Select(a => a.Address))
                .ProjectTo<AddressDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            org.Banks = await _context.Organizations
                .AsNoTracking()
                .Where(m => m.OrgId == guid)
                .SelectMany(pb => pb.Banks.Select(a => a.BankAccount))
                .ProjectTo<BankAccountDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return org;

        }

        public async Task<OrganizationDto> SaveAsync(OrganizationDto orgVM)
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
                    .Include(p => p.Banks).ThenInclude(pb => pb.BankAccount)
                    .FirstOrDefaultAsync();
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
                    // delete all banks that no longer exists
                    foreach (var dbOrgBank in dbOrg.Banks)
                    {
                        var dbBank = dbOrgBank.BankAccount;
                        if (org.Banks.All(pb => pb.BankAccount.BankAcctId != dbBank.BankAcctId))
                            _context.BankAccounts.Remove(dbBank);
                    }
                    // copy current (incoming) values to db
                    org.DateModified = DateTime.Now;
                    _context.Entry(dbOrg).CurrentValues.SetValues(org);

                    #region Contacts
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
                    #endregion

                    #region Addresses
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
                    #endregion

                    #region Banks
                    var bankPairs = from curr in org.Banks.Select(pb => pb.BankAccount)
                                    join db in dbOrg.Banks.Select(pb => pb.BankAccount)
                                      on curr.BankAcctId equals db.BankAcctId into grp
                                    from db in grp.DefaultIfEmpty()
                                    select new { curr, db };
                    foreach (var pair in bankPairs)
                    {
                        if (pair.db != null)
                            _context.Entry(pair.db).CurrentValues.SetValues(pair.curr);
                        else
                        {
                            //_context.Set<Address>().Add(pair.curr);
                            _context.Set<OrgBank>().Add(new OrgBank
                            {
                                OrgId = org.OrgId,
                                Organization = org,
                                BankAcctId = pair.curr.BankAcctId,
                                BankAccount = pair.curr
                            });
                        }
                    }
                    #endregion Banks
                }
            }

            await _context.SaveChangesAsync();
            orgVM = _mapper.Map<OrganizationDto>(org);
            return orgVM;
        }

        public async Task<bool> DeleteAsync(string orgId)
        {
            if (string.IsNullOrEmpty(orgId))
                throw new ArgumentNullException("orgId", "Org id is missing");

            Guid guid = new Guid(orgId);
            var org = await _context.Organizations
                .Include(o => o.Contacts).ThenInclude(oc => oc.Contact)
                .Include(o => o.Addresses).ThenInclude(oa => oa.Address)
                .Include(p => p.Banks).ThenInclude(pb => pb.BankAccount)
                .SingleOrDefaultAsync(o => o.OrgId == guid);
            if (org != null)
            {
                foreach (var orgContact in org.Contacts)
                    _context.Contacts.Remove(orgContact.Contact);
                foreach (var orgAddress in org.Addresses)
                    _context.Addresses.Remove(orgAddress.Address);
                foreach (var orgBank in org.Banks)
                    _context.BankAccounts.Remove(orgBank.BankAccount);
                _context.Organizations.Remove(org);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Organization not found");
        }

        public bool Exists(string id)
        {
            Guid guid = new Guid(id);
            return _context.Organizations.Any(o => o.OrgId == guid);
        }

        public async Task<bool> ChangeStatus(string orgId, bool active)
        {
            if (string.IsNullOrEmpty(orgId))
                throw new ArgumentNullException("orgId", "Org id is missing");

            Guid guid = new Guid(orgId);
            var org = await _context.Organizations
                .FindAsync(guid);
            if (org != null)
            {
                org.Active = active;
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Organization not found");
        }
    }
}