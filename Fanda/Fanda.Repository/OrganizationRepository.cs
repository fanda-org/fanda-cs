using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fanda.Models;
using Fanda.Models.Context;
using Fanda.Dto;
using Fanda.Dto.Base;
using Fanda.Repository.Base;
using Fanda.Repository.Extensions;
using Fanda.Repository.Utilities;
using Fanda.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Fanda.Repository
{
    public interface IOrganizationRepository : IRepository<OrganizationDto>,
        IRepositoryChildData<OrgChildrenDto>,
        IRepositoryChildList<OrgListDto>,
        IRepositoryChildList<OrgYearListDto>
    {
        Task<bool> MapUserAsync(Guid orgId, Guid userId);
        Task<bool> UnmapUserAsync(Guid orgId, Guid userId);
    }

    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public OrganizationRepository(FandaContext context, IMapper mapper, IUserRepository userRepository)
        {
            _context = context;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<OrganizationDto> GetByIdAsync(Guid id, bool includeChildren = false)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is missing");
            }

            OrganizationDto org = await _context.Organizations
                .AsNoTracking()
                .ProjectTo<OrganizationDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (org == null)
            {
                throw new KeyNotFoundException("Organization not found");
            }
            else if (!includeChildren)
            {
                return org;
            }

            org.Contacts = await _context.Organizations
                .AsNoTracking()
                .Where(m => m.Id == id)
                .SelectMany(oc => oc.OrgContacts.Select(c => c.Contact))
                .ProjectTo<ContactDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            org.Addresses = await _context.Organizations
                .AsNoTracking()
                .Where(m => m.Id == id)
                .SelectMany(oa => oa.OrgAddresses.Select(a => a.Address))
                .ProjectTo<AddressDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return org;
        }

        public async Task<OrgChildrenDto> GetChildrenByIdAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is missing");
            }

            var org = new OrgChildrenDto
            {
                Contacts = await _context.Organizations
                .AsNoTracking()
                .Where(m => m.Id == id)
                .SelectMany(oc => oc.OrgContacts.Select(c => c.Contact))
                .ProjectTo<ContactDto>(_mapper.ConfigurationProvider)
                .ToListAsync(),
                Addresses = await _context.Organizations
                .AsNoTracking()
                .Where(m => m.Id == id)
                .SelectMany(oa => oa.OrgAddresses.Select(a => a.Address))
                .ProjectTo<AddressDto>(_mapper.ConfigurationProvider)
                .ToListAsync()
            };
            return org;
        }

        public async Task<OrganizationDto> SaveAsync(OrganizationDto model)
        {
            Organization org = _mapper.Map<Organization>(model);
            if (org.Id == Guid.Empty)
            {
                org.DateCreated = DateTime.Now;
                org.DateModified = null;
                await _context.Organizations.AddAsync(org);
            }
            else
            {
                Organization dbOrg = await _context.Organizations
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
                    // delete all contacts that are no longer exists
                    foreach (OrgContact dbOrgContact in dbOrg.OrgContacts)
                    {
                        Contact dbContact = dbOrgContact.Contact;
                        if (org.OrgContacts.All(oc => oc.Contact.Id != dbContact.Id))
                        {
                            _context.Contacts.Remove(dbContact);
                        }
                    }
                    // delete all addresses that are no longer exists
                    foreach (OrgAddress dbOrgAddress in dbOrg.OrgAddresses)
                    {
                        Address dbAddress = dbOrgAddress.Address;
                        if (org.OrgAddresses.All(oa => oa.Address.Id != dbAddress.Id))
                        {
                            _context.Addresses.Remove(dbAddress);
                        }
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
                        {
                            _context.Entry(pair.db).CurrentValues.SetValues(pair.curr);
                            _context.Contacts.Update(pair.db);
                        }
                        else
                        {
                            var orgContact = new OrgContact
                            {
                                OrgId = org.Id,
                                Organization = org,
                                ContactId = pair.curr.Id,
                                Contact = pair.curr
                            };
                            dbOrg.OrgContacts.Add(orgContact);
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
                        {
                            _context.Entry(pair.db).CurrentValues.SetValues(pair.curr);
                            _context.Addresses.Update(pair.db);
                        }
                        else
                        {
                            var orgAddress = new OrgAddress
                            {
                                OrgId = org.Id,
                                Organization = org,
                                AddressId = pair.curr.Id,
                                Address = pair.curr
                            };
                            dbOrg.OrgAddresses.Add(orgAddress);
                        }
                    }

                    _context.Organizations.Update(dbOrg);
                    #endregion
                }
            }

            await _context.SaveChangesAsync();
            model = _mapper.Map<OrganizationDto>(org);
            return model;
        }

        public async Task<DtoErrors> ValidateAsync(OrganizationDto model)
        {
            // Reset validation errors
            model.Errors.Clear();

            #region Formatting: Cleansing and formatting
            model.Code = model.Code.ToUpper();
            model.Name = model.Name.TrimExtraSpaces();
            model.Description = model.Description.TrimExtraSpaces();
            #endregion

            #region Validation: Dupllicate
            // Check code duplicate
            var duplCode = new Duplicate { Field = DuplicateField.Code, Value = model.Code, Id = model.Id };
            if (await ExistsAsync(duplCode))
            {
                model.Errors.Add(nameof(model.Code), $"{nameof(model.Code)} '{model.Code}' already exists");
            }
            // Check name duplicate
            var duplName = new Duplicate { Field = DuplicateField.Name, Value = model.Name, Id = model.Id };
            if (await ExistsAsync(duplName))
            {
                model.Errors.Add(nameof(model.Name), $"{nameof(model.Name)} '{model.Name}' already exists");
            }
            #endregion

            return model.Errors;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is missing");
            }

            Organization org = await _context.Organizations
                .Include(o => o.OrgContacts).ThenInclude(oc => oc.Contact)
                .Include(o => o.OrgAddresses).ThenInclude(oa => oa.Address)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (org != null)
            {
                foreach (OrgContact orgContact in org.OrgContacts)
                {
                    _context.Contacts.Remove(orgContact.Contact);
                }
                foreach (OrgAddress orgAddress in org.OrgAddresses)
                {
                    _context.Addresses.Remove(orgAddress.Address);
                }
                _context.Organizations.Remove(org);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Organization not found");
        }

        public async Task<bool> ChangeStatusAsync(ActiveStatus status)
        {
            if (status.Id == null || status.Id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is missing");
            }

            Organization org = await _context.Organizations
                .FindAsync(status.Id);
            if (org != null)
            {
                org.Active = status.Active;
                _context.Organizations.Update(org);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Organization not found");
        }

        public Task<bool> ExistsAsync(Duplicate data) => _context.ExistsAsync<Organization>(data);

        #region List
        public IQueryable<OrgListDto> GetAll(Guid userId)
        {
            if (userId == null || userId == Guid.Empty)
                throw new ArgumentNullException("userId", "User id is missing");

            IQueryable<OrgListDto> query = _context.Organizations
                .Include(o => o.OrgUsers)
                .AsNoTracking()
                .Where(o => o.OrgUsers.Select(ou => ou.UserId).Any(uid => uid == userId))
                .ProjectTo<OrgListDto>(_mapper.ConfigurationProvider);
            return GetAll(query);
        }
        IQueryable<OrgYearListDto> IRepositoryChildList<OrgYearListDto>.GetAll(Guid userId)
        {
            if (userId == null || userId == Guid.Empty)
                throw new ArgumentNullException("userId", "User id is missing");

            IQueryable<OrgYearListDto> query = _context.Organizations
                .Include(o => o.AccountYears)
                .Include(o => o.OrgUsers)
                .AsNoTracking()
                .Where(o => o.OrgUsers.Select(ou => ou.UserId).Any(uid => uid == userId))
                .ProjectTo<OrgYearListDto>(_mapper.ConfigurationProvider);
            return GetAll(query);
        }
        private IQueryable<T> GetAll<T>(IQueryable<T> query)
            where T : BaseListDto
        {
            query = query.Where(c => c.Code != "FANDA");
            return query;
        }

        public async Task<bool> MapUserAsync(Guid orgId, Guid userId) => await _userRepository.MapOrgAsync(userId, orgId);

        public async Task<bool> UnmapUserAsync(Guid orgId, Guid userId) => await _userRepository.UnmapOrgAsync(userId, orgId);
        #endregion
    }
}