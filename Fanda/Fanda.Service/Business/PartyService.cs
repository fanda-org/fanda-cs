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
        IQueryable<PartyViewModel> GetAll(string orgId/*, bool? active*/);
        Task<PartyViewModel> GetByIdAsync(string partyId);
        Task<bool> SaveAsync(string orgId, PartyViewModel partyVM);
        Task<bool> DeleteAsync(string partyId);
        Task<bool> ChangeStatus(string categoryId, bool active);
        string ErrorMessage { get; }
    }

    public class PartyService : IPartyService
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;

        public PartyService(FandaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string ErrorMessage { get; private set; }

        public IQueryable<PartyViewModel> GetAll(string orgId/*, bool? active*/)
        {
            if (string.IsNullOrEmpty(orgId))
                throw new ArgumentNullException("orgId", "Org id is missing");

            Guid guid = new Guid(orgId);
            var parties = _context.Parties
                .Include(p => p.Category)
                .AsNoTracking()
                .Where(p => p.OrgId == guid)
                //.Where(p => p.Active == ((active == null) ? p.Active : active))
                .ProjectTo<PartyViewModel>(_mapper.ConfigurationProvider);
            return parties;
        }

        public async Task<PartyViewModel> GetByIdAsync(string partyId)
        {
            if (string.IsNullOrEmpty(partyId))
                throw new ArgumentNullException("partyId", "Party id is missing");

            var party = await _context.Parties
                .AsNoTracking()
                .ProjectTo<PartyViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(p => p.PartyId == partyId);

            if (party != null)
            {
                Guid guid = new Guid(partyId);
                party.Contacts = await _context.Parties
                    .AsNoTracking()
                    .Where(m => m.PartyId == guid)
                    .SelectMany(pc => pc.Contacts.Select(c => c.Contact))
                    .ProjectTo<ContactViewModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                party.Addresses = await _context.Parties
                    .AsNoTracking()
                    .Where(m => m.PartyId == guid)
                    .SelectMany(pa => pa.Addresses.Select(a => a.Address))
                    .ProjectTo<AddressViewModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                party.Banks = await _context.Parties
                    .AsNoTracking()
                    .Where(m => m.PartyId == guid)
                    .SelectMany(pb => pb.Banks.Select(a => a.BankAccount))
                    .ProjectTo<BankAccountViewModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                return party;
            }
            throw new KeyNotFoundException("Customer/Supplier not found");
        }

        public async Task<bool> SaveAsync(string orgId, PartyViewModel partyVM)
        {
            if (string.IsNullOrEmpty(orgId))
                throw new ArgumentNullException("orgId", "Org id is missing");

            var party = _mapper.Map<Party>(partyVM);
            party.OrgId = new Guid(orgId);

            if (party.PartyId == Guid.Empty)
            {
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
                    .Include(p => p.Banks).ThenInclude(pb => pb.BankAccount)
                    .FirstOrDefaultAsync();
                if (dbParty == null)
                {
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
                    // delete all banks that no longer exists
                    foreach (var dbPartyBank in dbParty.Banks)
                    {
                        var dbBank = dbPartyBank.BankAccount;
                        if (party.Banks.All(pb => pb.BankAccount.BankAcctId != dbBank.BankAcctId))
                            _context.BankAccounts.Remove(dbBank);
                    }
                    // copy current (incoming) values to db
                    party.DateModified = DateTime.Now;
                    _context.Entry(dbParty).CurrentValues.SetValues(party);

                    #region Contacts
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
                    #endregion Contacts

                    #region Addresses
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
                    #endregion Addresses

                    #region Banks
                    var bankPairs = from curr in party.Banks.Select(pb => pb.BankAccount)
                                    join db in dbParty.Banks.Select(pb => pb.BankAccount)
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
                            _context.Set<PartyBank>().Add(new PartyBank
                            {
                                PartyId = party.PartyId,
                                Party = party,
                                BankAcctId = pair.curr.BankAcctId,
                                BankAccount = pair.curr
                            });
                        }
                    }
                    #endregion Banks
                }
            }

            await _context.SaveChangesAsync();
            //partyVM = _mapper.Map<PartyViewModel>(party);
            return true; //partyVM;
        }

        public async Task<bool> DeleteAsync(string partyId)
        {
            if (string.IsNullOrEmpty(partyId))
                throw new ArgumentNullException("partyId", "Party id is missing");

            Guid guid = new Guid(partyId);
            var party = await _context.Parties
                .Include(p => p.Contacts).ThenInclude(pc => pc.Contact)
                .Include(p => p.Addresses).ThenInclude(pa => pa.Address)
                .Include(p => p.Banks).ThenInclude(pb => pb.BankAccount)
                .FirstOrDefaultAsync(p => p.PartyId == guid);
            if (party != null)
            {
                foreach (var partyContact in party.Contacts)
                    _context.Contacts.Remove(partyContact.Contact);
                foreach (var partyAddress in party.Addresses)
                    _context.Addresses.Remove(partyAddress.Address);
                foreach (var partyBank in party.Banks)
                    _context.BankAccounts.Remove(partyBank.BankAccount);
                _context.Parties.Remove(party);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Customer/Supplier not found");
        }

        public async Task<bool> ChangeStatus(string partyId, bool active)
        {
            if (string.IsNullOrEmpty(partyId))
                throw new ArgumentNullException("partyId", "Party id is missing");

            Guid guid = new Guid(partyId);
            var party = await _context.Parties
                .FindAsync(guid);
            if (party != null)
            {
                party.Active = active;
                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Customer/Supplier not found");
        }
    }
}