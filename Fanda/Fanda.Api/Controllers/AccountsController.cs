using Fanda.Service.Access;
using Fanda.ViewModel.Access;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Fanda.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IAccountService _service;

        public AccountsController(IAccountService service)
        {
            _service = service;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(IdentityResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            IdentityResult result = await _service.RegisterAsync(model);
            if (!result.Succeeded)
            {
                var errors = string.Join(",", result.Errors);
                ModelState.AddModelError(string.Empty, errors);
                return BadRequest(result.Errors);
            }
            else
                return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ViewModel.Access.SignInResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            ViewModel.Access.SignInResult result = await _service.LoginAsync(model);
            if (result.Succeeded)
            {
                //_logger.LogInformation("User logged in.");
                //return RedirectToLocal(returnUrl);
                return Ok(result);
            }
            //if (result.RequiresTwoFactor)
            //{
            //    return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, model.RememberMe });
            //}
            if (result.IsLockedOut)
            {
                //_logger.LogWarning("User account locked out.");
                //return RedirectToAction(nameof(Lockout));
                return StatusCode((int)HttpStatusCode.Forbidden, result);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return NotFound(new { ErrorMessage = "Invalid login attempt" });
            }

            // If we got this far, something failed, redisplay form
            //return BadRequest(new { ErrorMessage = "Invalid input" });
        }

        [HttpPost]
        public async Task Logout()
        {
            await _service.LogoutAsync();
        }
    }
}