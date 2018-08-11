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
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        // GET: api/Products
        [HttpGet]
        //[HttpGet(".{format}"), FormatFilter]
        public async Task<IActionResult> GetProducts([FromQuery] Guid orgId, [FromQuery] bool? active)
        {
            var products = await _service.GetAllAsync(orgId, active);
            if (products != null)
                return Ok(products);
            return StatusCode(500, _service.ErrorMessage);
        }

        // GET: api/Products/5
        [HttpGet("{productId}")]
        //[HttpGet("{id}.{format}"), FormatFilter]
        public async Task<IActionResult> GetProduct([FromRoute] Guid productId)
        {
            var product = await _service.GetByIdAsync(productId);
            if (product != null)
                return Ok(product);

            if (_service.ErrorMessage.Contains("not found"))
                return NotFound(_service.ErrorMessage);
            return StatusCode(500, _service.ErrorMessage);
        }

        // POST: api/Products/5
        [HttpPost("{orgId}")]
        public async Task<IActionResult> PostProduct([FromRoute] Guid orgId, [FromBody] ProductViewModel productVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            productVM = await _service.SaveAsync(orgId, productVM);
            if (productVM != null)
                return CreatedAtAction("GetProduct", new { productId = productVM.ProductId }, productVM);
            return StatusCode(500, _service.ErrorMessage);
        }

        // DELETE: api/Products/5
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid productId)
        {
            if (await _service.DeleteAsync(productId))
                return NoContent();

            if (_service.ErrorMessage.Contains("not found"))
                return NotFound(_service.ErrorMessage);
            return StatusCode(500, _service.ErrorMessage);
        }
    }
}