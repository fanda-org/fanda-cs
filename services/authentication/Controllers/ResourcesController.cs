namespace Fanda.Authentication.Controllers
{
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using System.Web;
    using Fanda.Core.Base;
    using Fanda.Core.Auth;
    using Fanda.Infrastructure.Auth;
    using Fanda.Infrastructure.Base;
    using Fanda.Infrastructure.Extensions;
    using Fanda.Shared;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class ResourcesController : BaseController
    {
        private readonly IResourceRepository repository;
        public ResourcesController(IResourceRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                NameValueCollection queryString = HttpUtility.ParseQueryString(Request.QueryString.Value);
                int page = string.IsNullOrEmpty(queryString["page"]) ? 1 : Convert.ToInt32(queryString["page"]);
                int pageSize = string.IsNullOrEmpty(queryString["pageSize"]) ? 100 : Convert.ToInt32(queryString["pageSize"]);
                var response = await repository
                    .GetPaged(Guid.Empty,
                        new Query
                        {
                            Filter = queryString["filter"],
                            FilterArgs = queryString["filterArgs"]?.Split(','),
                            Page = page,
                            PageSize = pageSize,
                            Search = queryString["search"],
                            Sort = queryString["sort"],
                        });
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(DataResponse.Failure(ex.Message));
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById([Required, FromRoute] Guid id, [FromQuery] bool include)
        {
            try
            {
                var resource = await repository.GetByIdAsync(id, include);
                if (resource != null)
                {
                    return Ok(DataResponse<ResourceDto>.Succeeded(resource));
                }
                return NotFound(DataResponse.Failure($"Resource id '{id}' not found"));
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is ArgumentNullException || ex is ArgumentException)
                {
                    return BadRequest(DataResponse.Failure("Invalid resource id"));
                }
                else if (ex is NotFoundException)
                {
                    return NotFound(DataResponse.Failure("Resource not found"));
                }
                else
                {
                    return InternalServerError(DataResponse.Failure(ex.Message));
                }
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(ResourceDto model)
        {
            try
            {
                #region Validation
                var validationResult = await repository.ValidateAsync(model);
                #endregion
                if (validationResult.IsValid)
                {
                    var resource = await repository.CreateAsync(model);
                    return CreatedAtAction(nameof(GetById), new { id = model.Id, include = false },
                        DataResponse<ResourceDto>.Succeeded(resource));
                }
                else
                {
                    return BadRequest(DataResponse.Failure(validationResult));
                }
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is ArgumentNullException || ex is ArgumentException)
                {
                    return BadRequest(DataResponse.Failure("Invalid resource id"));
                }
                else if (ex is NotFoundException)
                {
                    return NotFound(DataResponse.Failure("Resource not found"));
                }
                else
                {
                    return InternalServerError(DataResponse.Failure(ex.Message));
                }
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid id, ResourceDto model)
        {
            try
            {
                if (id != model.Id)
                {
                    return BadRequest(DataResponse.Failure("Resource id mismatch"));
                }
                #region Validation
                var validationResult = await repository.ValidateAsync(model);
                #endregion
                if (validationResult.IsValid)
                {
                    await repository.UpdateAsync(id, model);
                    return NoContent();
                }
                else
                {
                    return BadRequest(DataResponse.Failure(validationResult));
                }
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is ArgumentNullException || ex is ArgumentException)
                {
                    return BadRequest(DataResponse.Failure("Invalid resource id"));
                }
                else if (ex is NotFoundException)
                {
                    return NotFound(DataResponse.Failure("Resource not found"));
                }
                else
                {
                    return InternalServerError(DataResponse.Failure(ex.Message));
                }
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
                if (id == null || id == Guid.Empty)
                {
                    return BadRequest(DataResponse.Failure("Id is missing"));
                }
                var success = await repository.DeleteAsync(id);
                if (success)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound(DataResponse.Failure("Resource not found"));
                }
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is ArgumentNullException || ex is ArgumentException)
                {
                    return BadRequest(DataResponse.Failure("Invalid resource id"));
                }
                else if (ex is NotFoundException)
                {
                    return NotFound(DataResponse.Failure("Resource not found"));
                }
                else
                {
                    //return StatusCode(StatusCodes.Status500InternalServerError, DataResponse.Failure(ex.Message));
                    return InternalServerError(DataResponse.Failure(ex.Message));
                }
            }
        }

        [HttpPatch("active/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Active([Required, FromRoute] Guid id, [Required, FromQuery] bool active)
        {
            try
            {
                bool success = await repository.ChangeStatusAsync(new ActiveStatus
                {
                    Id = id,
                    Active = active
                });
                if (success)
                {
                    return Ok(DataResponse.Succeeded("Status changed successfully"));
                }
                return NotFound(DataResponse.Failure($"Resource id '{id}' not found"));
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is ArgumentNullException || ex is ArgumentException)
                {
                    return BadRequest(DataResponse.Failure("Invalid resource id"));
                }
                else if (ex is NotFoundException)
                {
                    return NotFound(DataResponse.Failure("Resource not found"));
                }
                else
                {
                    return InternalServerError(DataResponse.Failure(ex.Message));
                }
            }
        }

        [HttpGet("exists/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Exists([Required, FromRoute] Guid id, ExistsDto exists)
        {
            try
            {
                bool success = await repository.ExistsAsync(new ParentDuplicate
                {
                    Id = id,
                    Field = exists.Field,
                    Value = exists.Value
                });
                if (success)
                {
                    return Ok(DataResponse.Succeeded("Found"));
                }
                return NotFound(DataResponse.Failure("Not found"));
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is ArgumentNullException || ex is ArgumentException)
                {
                    return BadRequest(DataResponse.Failure("Invalid input"));
                }
                else if (ex is NotFoundException)
                {
                    return NotFound(DataResponse.Failure("Not found"));
                }
                else
                {
                    //return StatusCode(StatusCodes.Status500InternalServerError, DataResponse.Failure(ex.Message));
                    return InternalServerError(DataResponse.Failure(ex.Message));
                }
            }
        }

        [HttpPost("map")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Map([FromBody] ResourceActionDto resourceAction)
        {
            try
            {
                bool success = await repository.MapAction(resourceAction);
                if (success)
                {
                    return Ok(DataResponse.Succeeded("Mapped resource-action successfully"));
                }
                return NotFound(DataResponse.Failure($"Resource-action not found"));
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is ArgumentNullException || ex is ArgumentException)
                {
                    return BadRequest(DataResponse.Failure("Invalid resource-action id"));
                }
                else if (ex is NotFoundException)
                {
                    return NotFound(DataResponse.Failure("Resource-action not found"));
                }
                else
                {
                    return InternalServerError(DataResponse.Failure(ex.Message));
                }
            }
        }

        [HttpPost("unmap")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Unmap([FromBody] ResourceActionDto resourceAction)
        {
            try
            {
                bool success = await repository.UnmapAction(resourceAction);
                if (success)
                {
                    return Ok(DataResponse.Succeeded("Unmapped resource-action successfully"));
                }
                return NotFound(DataResponse.Failure($"Resource-action not found"));
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is ArgumentNullException || ex is ArgumentException)
                {
                    return BadRequest(DataResponse.Failure("Invalid resource-action id"));
                }
                else if (ex is NotFoundException)
                {
                    return NotFound(DataResponse.Failure("Resource-action not found"));
                }
                else
                {
                    return InternalServerError(DataResponse.Failure(ex.Message));
                }
            }
        }
    }
}