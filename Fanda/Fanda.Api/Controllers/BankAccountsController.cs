using Fanda.Common.Enums;
using Fanda.Service.Business;
using Fanda.ViewModel.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Fanda.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BankAccountsController : ControllerBase
    {
        private readonly IBankAccountService _service;

        public BankAccountsController(IBankAccountService service)
        {
            _service = service;
        }

        // GET: api/Banks
        [HttpGet]
        //[HttpGet(".{format}"), FormatFilter]
        public async Task<IActionResult> GetBankAccounts([FromQuery] AccountOwner owner, [FromQuery] Guid? ownerId, [FromQuery] bool? active)
        {
            var accts = await _service.GetAllAsync(owner, ownerId, active);
            if (accts != null)
                return Ok(accts);
            return StatusCode(500, _service.ErrorMessage);
        }

        // GET: api/Banks/5
        [HttpGet("{accountId}")]
        //[HttpGet("{id}.{format}"), FormatFilter]
        public async Task<IActionResult> GetBankAccount([FromRoute] Guid accountId)
        {
            var bankAccount = await _service.GetByIdAsync(accountId);
            if (bankAccount == null)
                return NotFound();
            return Ok(bankAccount);
        }

        // POST: api/Banks
        [HttpPost]
        public async Task<IActionResult> PostBankAccount([FromBody] BankAccountViewModel accountVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                accountVM = await _service.SaveAsync(accountVM);
                if (accountVM != null)
                    return CreatedAtAction("GetBankAccount", new { accountId = accountVM.BankAcctId }, accountVM);
                return StatusCode(500, _service.ErrorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/Banks/5
        [HttpDelete("{accountId}")]
        public async Task<IActionResult> DeleteBankAccount([FromRoute] Guid accountId)
        {
            if (await _service.DeleteAsync(accountId))
                return NoContent();

            if (_service.ErrorMessage.Contains("not found"))
                return NotFound(_service.ErrorMessage);
            return StatusCode(500, _service.ErrorMessage);
        }
    }
}