using Fanda.Dto;
using Fanda.Dto.ViewModels;
using Fanda.Repository;
using Fanda.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Fanda.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repository;

        public UsersController(IUserRepository repository)
        {
            _repository = repository;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest model)
        {
            var response = await _repository.Authenticate(model, IpAddress());

            if (response == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            SetTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await _repository.RefreshToken(refreshToken, IpAddress());

            if (response == null)
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            SetTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model)
        {
            // accept token from request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Token is required" });
            }

            var response = await _repository.RevokeToken(token, IpAddress());

            if (!response)
            {
                return NotFound(new { message = "Token not found" });
            }

            return Ok(new { message = "Token revoked" });
        }

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}

            string token = Guid.NewGuid().ToString();
            string callbackUrl = Url.Page(
                "/Users/ConfirmEmail",
                pageHandler: null,
                values: new { userId = model.Name, code = token },
                protocol: Request.Scheme
            );

            // save
            var userDto = await _repository.RegisterAsync(model, callbackUrl);
            return CreatedAtAction(nameof(GetById), userDto.Id); //Ok(response);
            //return RedirectToAction(nameof(Login));
        }

        // users/getall/5
        [HttpGet("getall/{orgId}")]
        public async Task<IActionResult> GetAll([FromRoute] Guid orgId)
        {
            var users = await _repository.GetAll(orgId)
                .ToListAsync();
            return Ok(users);
        }

        // users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _repository.GetByIdAsync(id);
            return Ok(user);
        }

        [HttpGet("{id}/refresh-tokens")]
        public async Task<IActionResult> GetRefreshTokens(Guid id)
        {
            var user = await _repository.GetByIdAsync(id);
            return Ok(user.RefreshTokens);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserDto model)
        {
            await _repository.CreateAsync(model);
            return CreatedAtAction(nameof(GetById), model.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Guid userId, UserDto model)
        {
            try
            {
                if (userId != model.Id)
                    return BadRequest(new { message = "User Id mismatch" });

                var user = await _repository.GetByIdAsync(userId);
                if (user == null)
                    return NotFound(new { message = "User not found" });

                // save 
                //model.Password = password;
                await _repository.UpdateAsync(userId, model);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{orgId}/{userId}")]
        public async Task<IActionResult> Delete(Guid orgId, Guid userId)
        {
            await _repository.UnmapOrgAsync(userId, orgId);
            return Ok();
        }

        #region helper methods
        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string IpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return Request.Headers["X-Forwarded-For"];
            }
            else
            {
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
        }
        #endregion
    }
}
