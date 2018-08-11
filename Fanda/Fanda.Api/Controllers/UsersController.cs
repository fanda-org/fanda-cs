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
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService userService)
        {
            _service = userService;
        }

        // GET: api/Users/1
        [HttpGet]
        //[HttpGet("{orgId?}")]
        public async Task<IActionResult> GetUsers([FromQuery] Guid? orgId, [FromQuery] bool? active)
        {
            var users = await _service.GetAllAsync(orgId, active);
            if (users != null)
                return Ok(users);
            return StatusCode(500, _service.ErrorMessage);
        }

        // GET: api/Users/1?userId=5
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser([FromRoute] Guid userId)
        {
            var user = await _service.GetByIdAsync(userId);
            if (user != null)
                return Ok(user);
            if (_service.ErrorMessage.Contains("not found"))
                return NotFound(_service.ErrorMessage);
            return StatusCode(500, _service.ErrorMessage);
        }

        // POST: api/Users
        [HttpPost("{orgId}")]
        public async Task<IActionResult> PostUser([FromRoute] Guid orgId, [FromBody] UserViewModel userVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                userVM = await _service.SaveAsync(orgId, userVM);
                if (userVM != null)
                    return CreatedAtAction("GetUser", new { userId = userVM.UserId }, userVM);
                return StatusCode(500, _service.ErrorMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{orgId}/{userId}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid orgId, [FromRoute] Guid userId)
        {
            if (await _service.DeleteAsync(orgId, userId))
                return NoContent();
            if (_service.ErrorMessage.Contains("not found"))
                return NotFound(_service.ErrorMessage);
            return StatusCode(500, _service.ErrorMessage);
        }
    }
}