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
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IProductCategoryService _service;

        public ProductCategoriesController(IProductCategoryService service)
        {
            _service = service;
        }

        // GET: api/ProductCategories
        [HttpGet]
        //[HttpGet(".{format}"), FormatFilter]
        public async Task<IActionResult> GetProductCategories([FromQuery] Guid orgId, [FromQuery] bool? active)
        {
            var categories = await _service.GetAllAsync(orgId, active);
            if (categories != null)
                return Ok(categories);
            return StatusCode(500, _service.ErrorMessage);
        }

        // GET: api/ProductCategories/5
        [HttpGet("{categoryId}")]
        //[HttpGet("{id}.{format}"), FormatFilter]
        public async Task<IActionResult> GetProductCategory([FromRoute] Guid categoryId)
        {
            var category = await _service.GetByIdAsync(categoryId);
            if (category != null)
                return Ok(category);

            if (_service.ErrorMessage.Contains("not found"))
                return NotFound(_service.ErrorMessage);
            return StatusCode(500, _service.ErrorMessage);
        }

        // POST: api/ProductCategories/5
        [HttpPost("{orgId}")]
        public async Task<IActionResult> PostProductCategory([FromRoute] Guid orgId, [FromBody] ProductCategoryViewModel categoryVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            categoryVM = await _service.SaveAsync(orgId, categoryVM);
            if (categoryVM != null)
                return CreatedAtAction("GetProductCategory", new { categoryId = categoryVM.CategoryId }, categoryVM);
            return StatusCode(500, _service.ErrorMessage);
        }

        // DELETE: api/ProductCategories/5
        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteProductCategory([FromRoute] Guid categoryId)
        {
            if (await _service.DeleteAsync(categoryId))
                return NoContent();

            if (_service.ErrorMessage.Contains("not found"))
                return NotFound(_service.ErrorMessage);
            return StatusCode(500, _service.ErrorMessage);
        }
    }
}