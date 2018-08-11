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
    public class ProductSegmentsController : ControllerBase
    {
        private readonly IProductSegmentService _service;

        public ProductSegmentsController(IProductSegmentService service)
        {
            _service = service;
        }

        // GET: api/ProductSegments
        [HttpGet]
        //[HttpGet(".{format}"), FormatFilter]
        public async Task<IActionResult> GetProductSegments([FromQuery] Guid orgId, [FromQuery] bool? active)
        {
            var segments = await _service.GetAllAsync(orgId, active);
            if (segments != null)
                return Ok(segments);
            return StatusCode(500, _service.ErrorMessage);
        }

        // GET: api/ProductSegments/5
        [HttpGet("{segmentId}")]
        //[HttpGet("{id}.{format}"), FormatFilter]
        public async Task<IActionResult> GetProductSegment([FromRoute] Guid segmentId)
        {
            var segment = await _service.GetByIdAsync(segmentId);
            if (segment != null)
                return Ok(segment);

            if (_service.ErrorMessage.Contains("not found"))
                return NotFound(_service.ErrorMessage);
            return StatusCode(500, _service.ErrorMessage);
        }

        // POST: api/ProductSegments/5
        [HttpPost("{orgId}")]
        public async Task<IActionResult> PostProductSegment([FromRoute] Guid orgId, [FromBody] ProductSegmentViewModel segmentVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            segmentVM = await _service.SaveAsync(orgId, segmentVM);
            if (segmentVM != null)
                return CreatedAtAction("GetProductSegment", new { segmentId = segmentVM.SegmentId }, segmentVM);
            return StatusCode(500, _service.ErrorMessage);
        }

        // DELETE: api/ProductSegments/5
        [HttpDelete("{segmentId}")]
        public async Task<IActionResult> DeleteProductSegment([FromRoute] Guid segmentId)
        {
            if (await _service.DeleteAsync(segmentId))
                return NoContent();

            if (_service.ErrorMessage.Contains("not found"))
                return NotFound(_service.ErrorMessage);
            return StatusCode(500, _service.ErrorMessage);
        }
    }
}