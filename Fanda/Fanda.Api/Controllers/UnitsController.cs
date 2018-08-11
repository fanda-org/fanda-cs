using Fanda.Service.Commodity;
using Fanda.ViewModel.Commodity;
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
    public class UnitsController : ControllerBase
    {
        private readonly IUnitService _service;

        public UnitsController(IUnitService service)
        {
            _service = service;
        }

        // GET: api/Units
        [HttpGet]
        //[HttpGet(".{format}"), FormatFilter]
        public async Task<IActionResult> GetUnits([FromQuery] Guid orgId, [FromQuery] bool? active)
        {
            var units = await _service.GetAllAsync(orgId, active);
            if (units != null)
                return Ok(units);
            return StatusCode(500, _service.ErrorMessage);
        }

        // GET: api/Units/5
        [HttpGet("{unitId}")]
        //[HttpGet("{id}.{format}"), FormatFilter]
        public async Task<IActionResult> GetUnit([FromRoute] Guid unitId)
        {
            var unit = await _service.GetByIdAsync(unitId);
            if (unit != null)
                return Ok(unit);

            if (_service.ErrorMessage.Contains("not found"))
                return NotFound(_service.ErrorMessage);
            return StatusCode(500, _service.ErrorMessage);
        }

        // POST: api/Units/5
        [HttpPost("{orgId}")]
        public async Task<IActionResult> PostUnit([FromRoute] Guid orgId, [FromBody] UnitViewModel unitVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            unitVM = await _service.SaveAsync(orgId, unitVM);
            if (unitVM != null)
                return CreatedAtAction("GetUnit", new { unitId = unitVM.UnitId }, unitVM);
            return StatusCode(500, _service.ErrorMessage);
        }

        // DELETE: api/Units/5
        [HttpDelete("{unitId}")]
        public async Task<IActionResult> DeleteUnit([FromRoute] Guid unitId)
        {
            if (await _service.DeleteAsync(unitId))
                return NoContent();

            if (_service.ErrorMessage.Contains("not found"))
                return NotFound(_service.ErrorMessage);
            return StatusCode(500, _service.ErrorMessage);
        }
    }
}