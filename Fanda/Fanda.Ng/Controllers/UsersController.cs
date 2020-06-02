using Fanda.Base;
using Fanda.Dto;
using Fanda.Dto.ViewModels;
using Fanda.Repository;
using Fanda.Repository.Base;
using Fanda.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Fanda.Controllers
{
    //[EnableCors("AllowAll")]
    //[Authorize]
    //[Produces(MediaTypeNames.Application.Json)]
    //[ApiController]
    //[Route("api/[controller]")]
    public class UsersController : BaseController
    {
        private readonly IUserRepository _repository;

        public UsersController(IUserRepository repository)
        {
            _repository = repository;
        }

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest model)
        {
            try
            {
                var response = await _repository.Authenticate(model, IpAddress());
                if (response == null)
                {
                    return BadRequest(
                        SingleResponse<AuthenticateResponse>.Failure("Username or password is incorrect"));
                }

                SetTokenCookie(response.RefreshToken);

                return Ok(
                    SingleResponse<AuthenticateResponse>.Succeeded(response));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    SingleResponse<AuthenticateResponse>.Failure(ex.Message));
            }
        }

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];
                var response = await _repository.RefreshToken(refreshToken, IpAddress());

                if (response == null)
                {
                    return Unauthorized(
                        SingleResponse<AuthenticateResponse>.Failure("Invalid token"));
                }

                SetTokenCookie(response.RefreshToken);

                return Ok(
                    SingleResponse<AuthenticateResponse>.Succeeded(response));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    SingleResponse<AuthenticateResponse>.Failure(ex.Message));
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model)
        {
            try
            {
                // accept token from request body or cookie
                var token = model.Token ?? Request.Cookies["refreshToken"];

                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest(
                        Repository.Base.Response.Failure(errorMessage: "Token is required"));
                }

                var response = await _repository.RevokeToken(token, IpAddress());

                if (!response)
                {
                    return NotFound(
                        Repository.Base.Response.Failure(errorMessage: "Token not found"));
                }

                return Ok(Repository.Base.Response.Succeeded(message: "Token revoked"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    Repository.Base.Response.Failure(ex.Message));
            }
        }

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    return View(model);
                //}
                string token = Guid.NewGuid().ToString();
                string callbackUrl = Url.Page(
                    pageName: "/Users/ConfirmEmail",
                    pageHandler: null,
                    values: new { userName = model.Username, code = token },
                    protocol: Request.Scheme
                );
                // save
                var userDto = await _repository.RegisterAsync(model, callbackUrl);
                return CreatedAtAction(nameof(GetById), userDto.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    Repository.Base.Response.Failure(ex.Message));
            }
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
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetById(Guid userId)
        {
            var user = await _repository.GetByIdAsync(userId);
            return Ok(user);
        }

        [HttpGet("{userId}/refresh-tokens")]
        public async Task<IActionResult> GetRefreshTokens(Guid userId)
        {
            var tokens = await _repository.GetRefreshTokens(userId);
            return Ok(tokens);
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
                {
                    return BadRequest(new { message = "User Id mismatch" });
                }

                var user = await _repository.GetByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                // save 
                //model.Password = password;
                await _repository.UpdateAsync(userId, model);
                return Ok();
            }
            catch (BadRequestException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    Repository.Base.Response.Failure(ex.Message));
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
