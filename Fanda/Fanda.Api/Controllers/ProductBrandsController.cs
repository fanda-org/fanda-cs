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
    public class ProductBrandsController : ControllerBase
    {
        private readonly IProductBrandService _service;

        public ProductBrandsController(IProductBrandService service)
        {
            _service = service;
        }

        // GET: api/ProductBrands
        [HttpGet]
        //[HttpGet(".{format}"), FormatFilter]
        public async Task<IActionResult> GetProductBrands([FromQuery] Guid orgId, [FromQuery] bool? active)
        {
            var brands = await _service.GetAllAsync(orgId, active);
            if (brands != null)
                return Ok(brands);
            return StatusCode(500, _service.ErrorMessage);
        }

        // GET: api/ProductBrands/5
        [HttpGet("{brandId}")]
        //[HttpGet("{id}.{format}"), FormatFilter]
        public async Task<IActionResult> GetProductBrand([FromRoute] Guid brandId)
        {
            var brand = await _service.GetByIdAsync(brandId);
            if (brand != null)
                return Ok(brand);

            if (_service.ErrorMessage.Contains("not found"))
                return NotFound(_service.ErrorMessage);
            return StatusCode(500, _service.ErrorMessage);
        }

        // POST: api/ProductBrands/5
        [HttpPost("{orgId}")]
        public async Task<IActionResult> PostProductBrand([FromRoute] Guid orgId, [FromBody] ProductBrandViewModel brandVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            brandVM = await _service.SaveAsync(orgId, brandVM);
            if (brandVM != null)
                return CreatedAtAction("GetProductBrand", new { brandId = brandVM.BrandId }, brandVM);
            return StatusCode(500, _service.ErrorMessage);
        }

        // DELETE: api/ProductBrands/5
        [HttpDelete("{brandId}")]
        public async Task<IActionResult> DeleteProductBrand([FromRoute] Guid brandId)
        {
            if (await _service.DeleteAsync(brandId))
                return NoContent();

            if (_service.ErrorMessage.Contains("not found"))
                return NotFound(_service.ErrorMessage);
            return StatusCode(500, _service.ErrorMessage);
        }
    }
}