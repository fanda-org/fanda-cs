using AutoMapper;
using Fanda.Data;
using Fanda.Data.Context;
using Fanda.Dto;
using Fanda.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanda.Service
{
    public interface IBankService
    {
        Task<List<BankDto>> GetAllAsync(string id, bool? active);

        Task<BankDto> GetByIdAsync(string ledgerId);

        Task<BankDto> SaveAsync(BankDto dto);

        Task<bool> DeleteAsync(string ledgerId);

        string ErrorMessage { get; }
    }

    public class BankAccountService : IBankAccountService
    {
        private readonly FandaContext _context;
        private readonly IMapper _mapper;

        public BankAccountService(FandaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string ErrorMessage { get; private set; }

        public async Task<List<BankDto>> GetAllAsync(string id, bool? active)
        {
            IQueryable<Bank> acctQry;

            if (owner == AccountOwner.Party && id != null && id != Guid.Empty)
                acctQry = _context.Parties //_userManager.Users
                    .Where(p => p.PartyId == id)
                    .SelectMany(p => p.Banks.Select(ou => ou.BankAccount));
            else if (owner == AccountOwner.Organization && id != null && id != Guid.Empty)
                acctQry = _context.Organizations //_userManager.Users
                    .Where(o => o.OrgId == id)
                    .SelectMany(o => o.Banks.Select(ou => ou.BankAccount));
            else
                acctQry = _context.BankAccounts;

            var acctList = _mapper.Map<List<BankDto>>(await acctQry
                .Where(u => u.Active == (active == null) ? u.Active : (bool)active)
                .AsNoTracking()
                .ToListAsync());

            if (owner != AccountOwner.None)
                acctList?.ForEach(b => { b.Owner = owner; b.OwnerId = id; });
            return acctList;
        }

        public async Task<BankDto> GetByIdAsync(Guid accountId)
        {
            var account = await _context.BankAccounts
                .Where(ba => ba.BankId == accountId)
                .Include(b => b.PartyBanks) //.ThenInclude(pb => pb.Party)
                .Include(b => b.OrgBanks)   //.ThenInclude(ob => ob.Organization)
                .Include(b => b.Contact)    //.ThenInclude(bc => bc.Contact)
                .Include(b => b.Address)    //.ThenInclude(ba => ba.Address)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (account != null)
            {
                var bavm = _mapper.Map<BankDto>(account);
                return bavm;
            }
            throw new KeyNotFoundException("Bank account not found");
        }

        public async Task<BankDto> SaveAsync(BankDto accountVM)
        {
            if (accountVM.Owner == AccountOwner.None || accountVM.OwnerId == null || accountVM.OwnerId == Guid.Empty)
                throw new ArgumentNullException("Owner", "Owner not set");

            Bank account;
            if (accountVM.BankAcctId == Guid.Empty /*accountDb == null*/)
            {
                account = _mapper.Map<Bank>(accountVM);
                account.DateCreated = DateTime.Now;
                account.DateModified = null;
                _context.BankAccounts.Add(account);
                if (accountVM.Owner == AccountOwner.Organization)
                    _context.Set<OrgBank>().Add(new OrgBank { OrgId = (Guid)accountVM.OwnerId, BankAcctId = account.BankId });
                else if (accountVM.Owner == AccountOwner.Party)
                    _context.Set<PartyBank>().Add(new PartyBank { PartyId = (Guid)accountVM.OwnerId, BankAcctId = account.BankId });
            }
            else
            {
                account = await _context.BankAccounts
                    .Include(b => b.OrgBanks)
                    .Include(b => b.PartyBanks)
                    .Include(b => b.Contact)
                    .Include(b => b.Address)
                    .SingleOrDefaultAsync(ba => ba.BankId == accountVM.BankAcctId);
                if (account == null)
                {
                    account = _mapper.Map<Bank>(accountVM);
                    account.DateCreated = DateTime.Now;
                    account.DateModified = null;
                    _context.BankAccounts.Add(account);

                    if (accountVM.Owner == AccountOwner.Organization)
                        _context.Set<OrgBank>().Add(new OrgBank { OrgId = (Guid)accountVM.OwnerId, BankAcctId = account.BankId });
                    else if (accountVM.Owner == AccountOwner.Party)
                        _context.Set<PartyBank>().Add(new PartyBank { PartyId = (Guid)accountVM.OwnerId, BankAcctId = account.BankId });
                }
                else
                {
                    if (account.Contact != null && accountVM.Contact == null)
                        _context.Contacts.Remove(account.Contact);
                    if (account.Address != null && accountVM.Address == null)
                        _context.Addresses.Remove(account.Address);

                    accountVM.DateModified = DateTime.Now;
                    _mapper.Map(accountVM, account);
                }
            }
            await _context.SaveChangesAsync();
            return _mapper.Map<BankDto>(account);
        }

        //private void UpdateContacts(BankAccount account, IEnumerable<ContactViewModel> contactsVM)
        //{
        //    if (account == null || contactsVM == null)
        //        return;
        //    try
        //    {
        //        int i = 0;
        //        for (; i < contactsVM.Count(); i++)
        //        {
        //            BankContact contact;
        //            if (i < account.Contacts.Count)
        //                contact = account.Contacts.Skip(i).Take(1).First();
        //            else
        //            {
        //                contact = new BankContact();
        //                contact.Contact = new Contact();
        //                contact.Bank = account;
        //            }
        //            var contactVM = contactsVM.Skip(i).Take(1).First();
        //            contact.Contact = contactVM.ToEntity(contact.Contact);
        //            // contact.Contact.ContactName = contactVM.ContactName;
        //            // contact.Contact.ContactTitle = contactVM.ContactTitle;
        //            // contact.Contact.ContactPhone = contactVM.ContactPhone;
        //            // contact.Contact.ContactEmail = contactVM.ContactEmail;
        //            if (contact.ContactId == Guid.Empty)
        //                account.Contacts.Add(contact);
        //        }
        //        for (int j = account.Contacts.Count - 1; j >= i; j--)
        //            account.Contacts.Remove(account.Contacts.Skip(i).Take(1).First());
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage = ex.InnerMessage();
        //    }
        //}
        //private void UpdateAddresses(BankAccount account, IEnumerable<AddressViewModel> addressesVM)
        //{
        //    if (account == null || addressesVM == null)
        //        return;
        //    try
        //    {
        //        int i = 0;
        //        for (; i < addressesVM.Count(); i++)
        //        {
        //            BankAddress address;
        //            if (i < account.Addresses.Count)
        //                address = account.Addresses.Skip(i).Take(1).First();
        //            else
        //            {
        //                address = new BankAddress();
        //                address.Address = new Address();
        //                address.Bank = account;
        //            }
        //            var addressVM = addressesVM.Skip(i).Take(1).First();
        //            address.Address = addressVM.ToEntity(address.Address);
        //            // address.Address.AddressLine1 = addressVM.AddressLine1;
        //            // address.Address.AddressLine2 = addressVM.AddressLine2;
        //            // address.Address.AddressType = addressVM.AddressType;
        //            // address.Address.City = addressVM.City;
        //            // address.Address.State = addressVM.State;
        //            // address.Address.Country = addressVM.Country;
        //            // address.Address.Postalcode = addressVM.Postalcode;
        //            if (address.AddressId == Guid.Empty)
        //                account.Addresses.Add(address);
        //        }
        //        for (int j = account.Addresses.Count - 1; j >= i; j--)
        //            account.Addresses.Remove(account.Addresses.Skip(i).Take(1).First());
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage = ex.InnerMessage();
        //    }
        //}

        public async Task<bool> DeleteAsync(Guid accountId)
        {
            var account = await _context.BankAccounts
                .Include(ba => ba.OrgBanks)
                .Include(ba => ba.PartyBanks)
                .Include(ba => ba.Contact)
                .Include(ba => ba.Address)
                .SingleOrDefaultAsync(ba => ba.BankId == accountId);
            if (account != null)
            {
                _context.Contacts.Remove(account.Contact);
                _context.Addresses.Remove(account.Address);
                _context.BankAccounts.Remove(account);

                await _context.SaveChangesAsync();
                return true;
            }
            throw new KeyNotFoundException("Bank account not found");
        }
    }
}