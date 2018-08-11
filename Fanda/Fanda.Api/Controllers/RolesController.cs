using Fanda.Service.Access;
using Fanda.ViewModel.Access;
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
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _service;

        public RolesController(IRoleService service)
        {
            _service = service;
        }

        // GET: api/Roles
        [HttpGet]
        //[HttpGet(".{format}"), FormatFilter]
        public async Task<IActionResult> GetRoles([FromQuery] bool? active = true)
        {
            try
            {
                var roles = await _service.GetAllAsync(active);
                if (roles != null)
                    return Ok(roles);

                return StatusCode(500, _service.ErrorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        //[HttpGet("{id}.{format}"), FormatFilter]
        public IActionResult GetRole([FromRoute] Guid id)
        {
            try
            {
                var roleVM = _service.GetByIdAsync(id);
                if (roleVM != null)
                    return Ok(roleVM);

                if (_service.ErrorMessage.Contains("not found"))
                    return NotFound(_service.ErrorMessage);
                return StatusCode(500, _service.ErrorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Roles
        [HttpPost]
        public async Task<IActionResult> PostRole([FromBody] RoleViewModel roleVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                roleVM = await _service.SaveAsync(roleVM);
                if (roleVM != null)
                    return CreatedAtAction("GetRole", new { id = roleVM.RoleId }, roleVM);
                return StatusCode(500, _service.ErrorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole([FromRoute] Guid id)
        {
            try
            {
                if (await _service.DeleteAsync(id))
                    return NoContent();
                if (_service.ErrorMessage.Contains("not found"))
                    return NotFound(_service.ErrorMessage);
                return StatusCode(500, _service.ErrorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}