using Fanda.Service.Business;
using Fanda.ViewModel.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Fanda.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrganizationsController : ControllerBase
    {
        private readonly IOrganizationService _service;

        public OrganizationsController(IOrganizationService service)
        {
            _service = service;
        }

        // GET: api/Organizations
        [HttpGet]
        //[SwaggerOperation("GetOrganizations")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(List<OrganizationViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrganizations([FromQuery] bool? active)
        {
            var orgs = await _service.GetAllAsync(active);
            if (orgs != null)
                return Ok(orgs);
            return StatusCode((int)HttpStatusCode.InternalServerError, _service.ErrorMessage);
        }

        // GET: api/Organizations/5
        [HttpGet("{orgId}")]
        //[SwaggerOperation("GetOrganization")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(OrganizationViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrganization([FromRoute] Guid orgId)
        {
            var org = await _service.GetByIdAsync(orgId);
            if (org != null)
                return Ok(org);

            if (_service.ErrorMessage.Contains("not found"))
                return NotFound(_service.ErrorMessage);
            return StatusCode((int)HttpStatusCode.InternalServerError, _service.ErrorMessage);
        }

        // POST: api/Organizations
        [HttpPost]
        //[SwaggerOperation("GetOrganizations")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(OrganizationViewModel), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> PostOrganization([FromBody] OrganizationViewModel orgVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            orgVM = await _service.SaveAsync(orgVM);
            if (orgVM != null)
                return CreatedAtAction("GetOrganization", new { orgId = orgVM.OrgId }, orgVM);
            return StatusCode((int)HttpStatusCode.InternalServerError, _service.ErrorMessage);
        }

        #region Commented

        // [HttpPost("{id}/[action]")]
        // public async Task<IActionResult> UpdateBanks([FromRoute] Guid id, [FromBody] List<BankAccountViewModel> banksVM)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return BadRequest(ModelState);
        //     }

        //     var org = await _context.Organizations
        //         .Include(o => o.Banks).ThenInclude(ob => ob.Bank).ThenInclude(b => b.Contacts).ThenInclude(bc => bc.Contact)
        //         .Include(o => o.Banks).ThenInclude(ob => ob.Bank).ThenInclude(b => b.Addresses).ThenInclude(ba => ba.Address)
        //         .SingleOrDefaultAsync(o => o.Id == id);
        //     if (org == null)
        //     {
        //         return NotFound();
        //     }
        //     int i = 0;
        //     for (; i < banksVM.Count; i++)
        //     {
        //         OrgBank orgBank;
        //         if (i < org.Banks.Count)
        //             orgBank = org.Banks.Skip(i).Take(1).First();
        //         else
        //         {
        //             orgBank = new OrgBank();
        //             orgBank.Organization = org;
        //             orgBank.Bank = new BankAccount();
        //             orgBank.Bank.Contacts = new List<BankContact>();
        //             orgBank.Bank.Addresses = new List<BankAddress>();
        //             orgBank.Bank.DateCreated = DateTime.Now;
        //         }
        //         var bankVM = banksVM[i];//.Skip(i).Take(1).First();
        //         orgBank.Bank.AccountNumber = bankVM.AccountNumber;
        //         orgBank.Bank.Name = bankVM.Name;
        //         orgBank.Bank.AccountType = bankVM.AccountType;
        //         orgBank.Bank.BranchCode = bankVM.BranchCode;
        //         orgBank.Bank.BranchName = bankVM.BranchName;
        //         orgBank.Bank.DateModified = DateTime.Now;
        //         orgBank.Bank.IfscCode = bankVM.IfscCode;
        //         orgBank.Bank.MicrCode = bankVM.MicrCode;
        //         orgBank.Bank.Active = bankVM.Active;
        //         orgBank.Bank.DateModified = DateTime.Now;
        //         // Bank Contact
        //         if (bankVM.Contact != null)
        //         {
        //             BankContact bankContact = orgBank.Bank.Contacts.FirstOrDefault();
        //             if (bankContact == null)
        //             {
        //                 bankContact = new BankContact();
        //                 bankContact.Bank = orgBank.Bank;
        //                 bankContact.Contact = new Contact();
        //             }
        //             bankContact.Contact.ContactName = bankVM.Contact.ContactName;
        //             bankContact.Contact.ContactTitle = bankVM.Contact.ContactTitle;
        //             bankContact.Contact.ContactPhone = bankVM.Contact.ContactPhone;
        //             bankContact.Contact.ContactEmail = bankVM.Contact.ContactEmail;
        //             if (bankContact.Contact.Id == Guid.Empty)
        //                 orgBank.Bank.Contacts.Add(bankContact);
        //         }
        //         // Bank Address
        //         if (bankVM.Address != null)
        //         {
        //             BankAddress bankAddress = orgBank.Bank.Addresses.FirstOrDefault();
        //             if (bankAddress == null)
        //             {
        //                 bankAddress = new BankAddress();
        //                 bankAddress.Bank = orgBank.Bank;
        //                 bankAddress.Address = new Address();
        //             }
        //             bankAddress.Address.AddressLine1 = bankVM.Address.AddressLine1;
        //             bankAddress.Address.AddressLine2 = bankVM.Address.AddressLine2;
        //             bankAddress.Address.AddressType = bankVM.Address.AddressType;
        //             bankAddress.Address.City = bankVM.Address.City;
        //             bankAddress.Address.State = bankVM.Address.State;
        //             bankAddress.Address.Country = bankVM.Address.Country;
        //             bankAddress.Address.Postalcode = bankVM.Address.Postalcode;
        //             if (bankAddress.Address.Id == Guid.Empty)
        //                 orgBank.Bank.Addresses.Add(bankAddress);
        //         }
        //         if (orgBank.BankId == Guid.Empty)
        //             org.Banks.Add(orgBank);
        //     }
        //     for (int j = org.Banks.Count - 1; j >= i; j--)
        //         org.Banks.Remove(org.Banks.Skip(i).Take(1).First());

        //     await _context.SaveChangesAsync();

        //     //return CreatedAtAction("GetOrganization", new { id = org.Id }, org);
        //     return NoContent();
        // }

        // [HttpPost("{id}/[action]")]
        // public async Task<IActionResult> UpdateUsers([FromRoute] Guid id, [FromBody] List<UserViewModel> usersVM)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return BadRequest(ModelState);
        //     }

        //     var org = await _context.Organizations
        //         .Include(o => o.Users)
        //         .SingleOrDefaultAsync(o => o.Id == id);
        //     if (org == null)
        //     {
        //         return NotFound();
        //     }
        //     int i = 0;
        //     for (; i < usersVM.Count; i++)
        //     {
        //         OrgUser user;
        //         if (i < org.Users.Count)
        //             user = org.Users.Skip(i).Take(1).First();
        //         else
        //         {
        //             user = new OrgUser();
        //             user.Organization = org;
        //             user.User = new User();
        //             user.User.DateCreated = DateTime.Now;
        //         }
        //         var userVM = usersVM[i];//.Skip(i).Take(1).First();
        //         user.User.UserName = userVM.UserName;
        //         user.User.Email = userVM.Email;
        //         user.User.FirstName = userVM.FirstName;
        //         user.User.LastName = userVM.LastName;
        //         user.User.Active = userVM.Active;
        //         user.User.DateLastLogin = userVM.DateLastLogin;
        //         user.User.DateModified = DateTime.Now;

        //         if (user.User.Id == Guid.Empty)
        //             org.Users.Add(user);
        //     }
        //     for (int j = org.Users.Count - 1; j >= i; j--)
        //         org.Users.Remove(org.Users.Skip(i).Take(1).First());

        //     await _context.SaveChangesAsync();

        //     //return CreatedAtAction("GetOrganization", new { id = org.Id }, org);
        //     return NoContent();
        // }

        #endregion Commented

        // DELETE: api/Organizations/5
        [HttpDelete("{orgId}")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteOrganization([FromRoute] Guid orgId)
        {
            if (await _service.DeleteAsync(orgId))
                return NoContent();

            if (_service.ErrorMessage.Contains("not found"))
                return NotFound(_service.ErrorMessage);
            return StatusCode((int)HttpStatusCode.InternalServerError, _service.ErrorMessage);
        }
    }
}