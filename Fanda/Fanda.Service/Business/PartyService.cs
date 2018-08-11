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
    public interface IPartyService
    {
        Task<List<PartyViewModel>> GetAllAsync(Guid orgId, bool? active);

        Task<PartyViewModel> GetByIdAsync(Guid partyId);

        Task<PartyViewModel> SaveAsync(Guid orgId, PartyViewModel partyVM);

        Task<bool> DeleteAsync(Guid partyId);

        string ErrorMessage { get; }
    }

    public class PartyService : IPartyService
    {
        private FandaContext _context;
        private IMapper _mapper;

        public PartyService(FandaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string ErrorMessage { get; private set; }

        public async Task<List<PartyViewModel>> GetAllAsync(Guid orgId, bool? active)
        {
            try
            {
                if (orgId == null || orgId == Guid.Empty)
                    throw new ArgumentNullException("orgId", "Org id is missing");

                var parties = await _context.Parties
                    .Where(p => p.OrgId == p.OrgId)
                    .Where(p => p.Active == ((active == null) ? p.Active : active))
                    .AsNoTracking()
                    .ToListAsync();

                return _mapper.Map<List<PartyViewModel>>(parties);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerMessage();
                return null;
            }
        }

        public async Task<PartyViewModel> GetByIdAsync(Guid partyId)
        {
            try
            {
                if (partyId == null || partyId == Guid.Empty)
                    throw new ArgumentNullException("partyId", "Party id is missing");

                var party = await _context.Parties
                    .ProjectTo<PartyViewModel>(_mapper.ConfigurationProvider)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(p => p.PartyId == partyId);

                if (party != null)
                {
                    party.Contacts = await _context.Parties
                        .Where(m => m.PartyId == partyId)
                        .SelectMany(oc => oc.Contacts.Select(c => c.Contact))
                        .ProjectTo<ContactViewModel>(_mapper.ConfigurationProvider)
                        .AsNoTracking()
                        .ToListAsync();
                    party.Addresses = await _context.Parties
                        .Where(m => m.PartyId == partyId)
                        .SelectMany(oa => oa.Addresses.Select(a => a.Address))
                        .ProjectTo<AddressViewModel>(_mapper.ConfigurationProvider)
                        .AsNoTracking()
                        .ToListAsync();

                    return party;
                }
                throw new KeyNotFoundException("Customer/Supplier not found");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerMessage();
                return null;
            }
        }

        public async Task<PartyViewModel> SaveAsync(Guid orgId, PartyViewModel partyVM)
        {
            try
            {
                if (orgId == null || orgId == Guid.Empty)
                    throw new ArgumentNullException("orgId", "Org id is missing");

                var party = _mapper.Map<Party>(partyVM);
                if (party.PartyId == Guid.Empty)
                {
                    party.OrgId = orgId;
                    party.DateCreated = DateTime.Now;
                    party.DateModified = null;
                    _context.Parties.Add(party);
                }
                else
                {
                    var dbParty = await _context.Parties
                        .Where(p => p.PartyId == party.PartyId)
                        .Include(p => p.Contacts).ThenInclude(pc => pc.Contact)
                        .Include(p => p.Addresses).ThenInclude(pa => pa.Address)
                        .SingleOrDefaultAsync();
                    if (dbParty == null)
                    {
                        party.OrgId = orgId;
                        party.DateCreated = DateTime.Now;
                        party.DateModified = null;
                        _context.Parties.Add(party);
                    }
                    else
                    {
                        // delete all contacts that no longer exists
                        foreach (var dbPartyContact in dbParty.Contacts)
                        {
                            var dbContact = dbPartyContact.Contact;
                            if (party.Contacts.All(oc => oc.Contact.ContactId != dbContact.ContactId))
                                _context.Contacts.Remove(dbContact);
                        }
                        // delete all addresses that no longer exists
                        foreach (var dbPartyAddress in dbParty.Addresses)
                        {
                            var dbAddress = dbPartyAddress.Address;
                            if (party.Addresses.All(oa => oa.Address.AddressId != dbAddress.AddressId))
                                _context.Addresses.Remove(dbAddress);
                        }
                        // copy current (incoming) values to db
                        party.DateModified = DateTime.Now;
                        _context.Entry(dbParty).CurrentValues.SetValues(party);
                        //_context.Entry(dbOrg).CurrentValues["DateModified"] = DateTime.Now;
                        var contactPairs = from curr in party.Contacts.Select(oc => oc.Contact)
                                           join db in dbParty.Contacts.Select(oc => oc.Contact)
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
                                _context.Set<PartyContact>().Add(new PartyContact
                                {
                                    PartyId = party.PartyId,
                                    Party = party,
                                    ContactId = pair.curr.ContactId,
                                    Contact = pair.curr
                                });
                            }
                        }
                        var addressPairs = from curr in party.Addresses.Select(oa => oa.Address)
                                           join db in dbParty.Addresses.Select(oa => oa.Address)
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
                                _context.Set<PartyAddress>().Add(new PartyAddress
                                {
                                    PartyId = party.PartyId,
                                    Party = party,
                                    AddressId = pair.curr.AddressId,
                                    Address = pair.curr
                                });
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
                partyVM = _mapper.Map<PartyViewModel>(party);
                return partyVM;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerMessage();
                return null;
            }
        }

        public async Task<bool> DeleteAsync(Guid partyId)
        {
            try
            {
                var party = await _context.Parties
                    .Include(p => p.Contacts).ThenInclude(pc => pc.Contact)
                    .Include(p => p.Addresses).ThenInclude(pa => pa.Address)
                    .SingleOrDefaultAsync(p => p.PartyId == partyId);
                if (party != null)
                {
                    foreach (var partyContact in party.Contacts)
                        _context.Contacts.Remove(partyContact.Contact);
                    foreach (var partyAddress in party.Addresses)
                        _context.Addresses.Remove(partyAddress.Address);
                    _context.Parties.Remove(party);
                    await _context.SaveChangesAsync();
                    return true;
                }
                throw new KeyNotFoundException("Customer/Supplier not found");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.InnerMessage();
                return false;
            }
        }
    }
}