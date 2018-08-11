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
    public class ProductVarietiesController : ControllerBase
    {
        private readonly IProductVarietyService _service;

        public ProductVarietiesController(IProductVarietyService service)
        {
            _service = service;
        }

        // GET: api/ProductVarieties
        [HttpGet]
        //[HttpGet(".{format}"), FormatFilter]
        public async Task<IActionResult> GetProductVarieties([FromQuery] Guid orgId, [FromQuery] bool? active)
        {
            var varieties = await _service.GetAllAsync(orgId, active);
            if (varieties != null)
                return Ok(varieties);
            return StatusCode(500, _service.ErrorMessage);
        }

        // GET: api/ProductVarieties/5
        [HttpGet("{varietyId}")]
        //[HttpGet("{id}.{format}"), FormatFilter]
        public async Task<IActionResult> GetProductVariety([FromRoute] Guid varietyId)
        {
            var variety = await _service.GetByIdAsync(varietyId);
            if (variety != null)
                return Ok(variety);

            if (_service.ErrorMessage.Contains("not found"))
                return NotFound(_service.ErrorMessage);
            return StatusCode(500, _service.ErrorMessage);
        }

        // POST: api/ProductVarieties/5
        [HttpPost("{orgId}")]
        public async Task<IActionResult> PostProductVariety([FromRoute] Guid orgId, [FromBody] ProductVarietyViewModel varietyVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            varietyVM = await _service.SaveAsync(orgId, varietyVM);
            if (varietyVM != null)
                return CreatedAtAction("GetProductVariety", new { varietyId = varietyVM.VarietyId }, varietyVM);
            return StatusCode(500, _service.ErrorMessage);
        }

        // DELETE: api/ProductVarieties/5
        [HttpDelete("{varietyId}")]
        public async Task<IActionResult> DeleteProductVariety([FromRoute] Guid varietyId)
        {
            if (await _service.DeleteAsync(varietyId))
                return NoContent();

            if (_service.ErrorMessage.Contains("not found"))
                return NotFound(_service.ErrorMessage);
            return StatusCode(500, _service.ErrorMessage);
        }
    }
}