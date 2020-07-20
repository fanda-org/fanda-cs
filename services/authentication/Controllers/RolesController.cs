namespace Fanda.Authentication.Controllers
{
    using Fanda.Infrastructure.Base;
    using Fanda.Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanda.Core.Auth;
    using Fanda.Core.Auth.ViewModels;
    using Fanda.Shared;

    //[EnableCors("_MyAllowedOrigins")]
    //[Authorize]
    //[Produces(MediaTypeNames.Application.Json)]
    //[ApiController]
    //[Route("api/[controller]")]
    public class RolesController : BaseController
    {
        private readonly IRoleRepository _repository;

        public RolesController(IRoleRepository repository)
        {
            _repository = repository;
        }

        // roles/all/5
        [HttpGet("all/{tenantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(Guid tenantId)
        {
            try
            {
                var roles = await _repository.GetAll(tenantId)
                    .ToListAsync();
                return Ok(DataResponse<IEnumerable<RoleListDto>>.Succeeded(roles));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    DataResponse.Failure(ex.Message));
            }
        }

        // roles/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var role = await _repository.GetByIdAsync(id);
                return Ok(DataResponse<RoleDto>.Succeeded(role));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    DataResponse.Failure(ex.Message));
            }
        }

        [HttpPost("{tenantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(Guid tenantId, RoleDto model)
        {
            try
            {
                var roleDto = await _repository.CreateAsync(tenantId, model);
                return CreatedAtAction(nameof(GetById), new { id = model.Id },
                    DataResponse<RoleDto>.Succeeded(roleDto));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    DataResponse.Failure(ex.Message));
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid id, RoleDto model)
        {
            try
            {
                if (id != model.Id)
                {
                    return BadRequest(DataResponse.Failure("Role Id mismatch"));
                }
                var role = await _repository.GetByIdAsync(id);
                if (role == null)
                {
                    return NotFound(DataResponse.Failure("Role not found"));
                }
                // save
                await _repository.UpdateAsync(id, model);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    DataResponse<string>.Failure(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var success = await _repository.DeleteAsync(id);
                if (success)
                {
                    return NoContent();
                }
                return NotFound(DataResponse.Failure("Role not found"));

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    DataResponse<string>.Failure(ex.Message));
            }
        }

        // [HttpPost("add-privilege")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        // public async Task<IActionResult> AddPrivilege([FromBody] PrivilegeDto model)
        // {
        //     try
        //     {
        //         bool success = await _repository.AddPrivilege(model);
        //         if (success)
        //         {
        //             return Ok(DataResponse.Succeeded("Mapped privilege successfully"));
        //         }
        //         return NotFound(DataResponse.Failure($"Privilege not found"));
        //     }
        //     catch (Exception ex)
        //     {
        //         if (ex is BadRequestException || ex is ArgumentNullException || ex is ArgumentException)
        //         {
        //             return BadRequest(DataResponse.Failure("Invalid privilege id"));
        //         }
        //         else if (ex is NotFoundException)
        //         {
        //             return NotFound(DataResponse.Failure("Privilege not found"));
        //         }
        //         else
        //         {
        //             return InternalServerError(DataResponse.Failure(ex.Message));
        //         }
        //     }
        // }

        // [HttpPost("remove-privilege")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        // public async Task<IActionResult> RemovePrivilege([FromBody] PrivilegeDto model)
        // {
        //     try
        //     {
        //         bool success = await _repository.RemovePrivilege(model);
        //         if (success)
        //         {
        //             return Ok(DataResponse.Succeeded("Unmapped privilege successfully"));
        //         }
        //         return NotFound(DataResponse.Failure($"Privilege not found"));
        //     }
        //     catch (Exception ex)
        //     {
        //         if (ex is BadRequestException || ex is ArgumentNullException || ex is ArgumentException)
        //         {
        //             return BadRequest(DataResponse.Failure("Invalid privilege id"));
        //         }
        //         else if (ex is NotFoundException)
        //         {
        //             return NotFound(DataResponse.Failure("Privilege not found"));
        //         }
        //         else
        //         {
        //             return InternalServerError(DataResponse.Failure(ex.Message));
        //         }
        //     }
        // }
    }
}
