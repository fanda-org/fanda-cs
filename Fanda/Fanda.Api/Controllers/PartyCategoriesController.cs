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
    public class PartyCategoriesController : ControllerBase
    {
        private readonly IPartyCategoryService _service;

        public PartyCategoriesController(IPartyCategoryService service)
        {
            _service = service;
        }

        // GET: api/PartyCategories
        [HttpGet]
        //[HttpGet(".{format}"), FormatFilter]
        public async Task<IActionResult> GetPartyCategories([FromQuery] Guid orgId, [FromQuery] bool? active)
        {
            var categories = await _service.GetAllAsync(orgId, active);
            if (categories != null)
                return Ok(categories);
            return StatusCode(500, _service.ErrorMessage);
        }

        // GET: api/PartyCategories/5
        [HttpGet("{categoryId}")]
        //[HttpGet("{id}.{format}"), FormatFilter]
        public async Task<IActionResult> GetPartyCategory([FromRoute] Guid categoryId)
        {
            var category = await _service.GetByIdAsync(categoryId);
            if (category != null)
                return Ok(category);

            if (_service.ErrorMessage.Contains("not found"))
                return NotFound(_service.ErrorMessage);
            return StatusCode(500, _service.ErrorMessage);
        }

        // POST: api/PartyCategories/5
        [HttpPost("{orgId}")]
        public async Task<IActionResult> PostPartyCategory([FromRoute] Guid orgId, [FromBody] PartyCategoryViewModel categoryVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            categoryVM = await _service.SaveAsync(orgId, categoryVM);
            if (categoryVM != null)
                return CreatedAtAction("GetPartyCategory", new { categoryId = categoryVM.CategoryId }, categoryVM);
            return StatusCode(500, _service.ErrorMessage);
        }

        // DELETE: api/PartyCategories/5
        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeletePartyCategory([FromRoute] Guid categoryId)
        {
            if (await _service.DeleteAsync(categoryId))
                return NoContent();

            if (_service.ErrorMessage.Contains("not found"))
                return NotFound(_service.ErrorMessage);
            return StatusCode(500, _service.ErrorMessage);
        }
    }
}