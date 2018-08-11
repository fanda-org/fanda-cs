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
    public class PartiesController : ControllerBase
    {
        private readonly IPartyService _service;

        public PartiesController(IPartyService service)
        {
            _service = service;
        }

        // GET: api/Parties
        [HttpGet]
        //[HttpGet(".{format}"), FormatFilter]
        public async Task<IActionResult> GetParties([FromQuery] Guid orgId, [FromQuery] bool? active)
        {
            var parties = await _service.GetAllAsync(orgId, active);
            if (parties != null)
                return Ok(parties);
            return StatusCode(500, _service.ErrorMessage);
        }

        // GET: api/Parties/5
        [HttpGet("{partyId}")]
        //[HttpGet("{id}.{format}"), FormatFilter]
        public async Task<IActionResult> GetParty([FromRoute] Guid partyId)
        {
            var party = await _service.GetByIdAsync(partyId);
            if (party != null)
                return Ok(party);

            if (_service.ErrorMessage.Contains("not found"))
                return NotFound(_service.ErrorMessage);
            return StatusCode(500, _service.ErrorMessage);
        }

        // POST: api/Parties/5
        [HttpPost("{orgId}")]
        public async Task<IActionResult> PostParty([FromRoute] Guid orgId, [FromBody] PartyViewModel partyVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            partyVM = await _service.SaveAsync(orgId, partyVM);
            if (partyVM != null)
                return CreatedAtAction("GetParty", new { partyId = partyVM.PartyId }, partyVM);
            return StatusCode(500, _service.ErrorMessage);
        }

        // DELETE: api/Parties/5
        [HttpDelete("{partyId}")]
        public async Task<IActionResult> DeleteParty([FromRoute] Guid partyId)
        {
            if (await _service.DeleteAsync(partyId))
                return NoContent();

            if (_service.ErrorMessage.Contains("not found"))
                return NotFound(_service.ErrorMessage);
            return StatusCode(500, _service.ErrorMessage);
        }
    }
}