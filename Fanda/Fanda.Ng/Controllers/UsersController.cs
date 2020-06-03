using Fanda.Base;
using Fanda.Dto;
using Fanda.Dto.ViewModels;
using Fanda.Repository;
using Fanda.Repository.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanda.Controllers
{
    //[EnableCors("_MyAllowedOrigins")]
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

        [HttpPost("authenticate")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest model)
        {
            try
            {
                var response = await _repository.Authenticate(model, IpAddress());
                if (response == null)
                {
                    return BadRequest(
                        DataResponse.Failure("Username or password is incorrect"));
                }

                SetTokenCookie(response.RefreshToken);
                return Ok(
                    DataResponse<AuthenticateResponse>.Succeeded(response));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    DataResponse.Failure(ex.Message));
            }
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];
                var response = await _repository.RefreshToken(refreshToken, IpAddress());
                if (response == null)
                {
                    return Unauthorized(
                        DataResponse.Failure("Invalid token"));
                }

                SetTokenCookie(response.RefreshToken);
                return Ok(
                    DataResponse<AuthenticateResponse>.Succeeded(response));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    DataResponse.Failure(ex.Message));
            }
        }

        [HttpPost("revoke-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model)
        {
            try
            {
                // accept token from request body or cookie
                var token = model.Token ?? Request.Cookies["refreshToken"];
                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest(
                        DataResponse.Failure(errorMessage: "Token is required"));
                }

                var response = await _repository.RevokeToken(token, IpAddress());
                if (!response)
                {
                    return NotFound(
                        DataResponse.Failure(errorMessage: "Token not found"));
                }
                return Ok(DataResponse.Succeeded(message: "Token revoked"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    DataResponse.Failure(ex.Message));
            }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                var validationResult = await _repository.ValidateAsync(model);
                if (!validationResult.IsValid)
                {
                    return BadRequest(
                        DataResponse.Failure(validationResult));
                }

                string token = Guid.NewGuid().ToString();
                string callbackUrl = Url.Page(
                    pageName: "/Users/ConfirmEmail",
                    pageHandler: null,
                    values: new { userName = model.Username, code = token },
                    protocol: Request.Scheme
                );
                // save
                var userDto = await _repository.RegisterAsync(model, callbackUrl);
                //return CreatedAtAction(nameof(GetById), userDto.Id);
                return CreatedAtAction(nameof(GetById), new { userId = userDto.Id },
                    DataResponse<UserDto>.Succeeded(userDto));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    DataResponse.Failure(ex.Message));
            }
        }

        // users/all/5
        [HttpGet("all/{orgId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(Guid orgId)
        {
            try
            {
                var users = await _repository.GetAll(orgId)
                    .ToListAsync();
                return Ok(DataResponse<IEnumerable<UserListDto>>.Succeeded(users));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    DataResponse.Failure(ex.Message));
            }
        }

        // users/5
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid userId)
        {
            try
            {
                var user = await _repository.GetByIdAsync(userId);
                return Ok(DataResponse<UserDto>.Succeeded(user));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    DataResponse.Failure(ex.Message));
            }
        }

        [HttpGet("{userId}/refresh-tokens")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRefreshTokens(Guid userId)
        {
            try
            {
                var tokens = await _repository.GetRefreshTokens(userId);
                return Ok(DataResponse<IEnumerable<RefreshTokenDto>>.Succeeded(tokens));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    DataResponse.Failure(ex.Message));
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(UserDto model)
        {
            try
            {
                var userDto = await _repository.CreateAsync(model);
                return CreatedAtAction(nameof(GetById), new { userId = model.Id },
                    DataResponse<UserDto>.Succeeded(userDto));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    DataResponse.Failure(ex.Message));
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid userId, UserDto model)
        {
            try
            {
                if (userId != model.Id)
                {
                    return BadRequest(DataResponse.Failure("User Id mismatch"));
                }
                var user = await _repository.GetByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(DataResponse.Failure("User not found"));
                }
                // save
                //model.Password = password;
                await _repository.UpdateAsync(userId, model);
                return NoContent(); //(DataResponse<UserDto>.Succeeded(UserDto));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    DataResponse<string>.Failure(ex.Message));
            }
        }

        [HttpDelete("{orgId}/{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid orgId, Guid userId)
        {
            try
            {
                await _repository.UnmapOrgAsync(userId, orgId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    DataResponse<string>.Failure(ex.Message));
            }
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
