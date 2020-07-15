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

    public class ApplicationsController : BaseController
    {
        private readonly IApplicationRepository repository;
        public ApplicationsController(IApplicationRepository repository)
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
                var app = await repository.GetByIdAsync(id, include);
                if (app != null)
                {
                    return Ok(DataResponse<ApplicationDto>.Succeeded(app));
                }
                return NotFound(DataResponse.Failure($"Application id '{id}' not found"));
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is ArgumentNullException || ex is ArgumentException)
                {
                    return BadRequest(DataResponse.Failure("Invalid application id"));
                }
                else if (ex is NotFoundException)
                {
                    return NotFound(DataResponse.Failure("Application not found"));
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
        public async Task<IActionResult> Create(ApplicationDto model)
        {
            try
            {
                #region Validation
                var validationResult = await repository.ValidateAsync(model);
                #endregion
                if (validationResult.IsValid)
                {
                    var app = await repository.CreateAsync(model);
                    return CreatedAtAction(nameof(GetById), new { id = model.Id, include = false },
                        DataResponse<ApplicationDto>.Succeeded(app));
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
                    return BadRequest(DataResponse.Failure("Invalid application id"));
                }
                else if (ex is NotFoundException)
                {
                    return NotFound(DataResponse.Failure("Application not found"));
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
        public async Task<IActionResult> Update(Guid id, ApplicationDto model)
        {
            try
            {
                if (id != model.Id)
                {
                    return BadRequest(DataResponse.Failure("Application id mismatch"));
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
                    return BadRequest(DataResponse.Failure("Invalid application id"));
                }
                else if (ex is NotFoundException)
                {
                    return NotFound(DataResponse.Failure("Application not found"));
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
                    return NotFound(DataResponse.Failure("Application not found"));
                }
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is ArgumentNullException || ex is ArgumentException)
                {
                    return BadRequest(DataResponse.Failure("Invalid application id"));
                }
                else if (ex is NotFoundException)
                {
                    return NotFound(DataResponse.Failure("Application not found"));
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
                return NotFound(DataResponse.Failure($"Application id '{id}' not found"));
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is ArgumentNullException || ex is ArgumentException)
                {
                    return BadRequest(DataResponse.Failure("Invalid application id"));
                }
                else if (ex is NotFoundException)
                {
                    return NotFound(DataResponse.Failure("Application not found"));
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
        public async Task<IActionResult> Map([FromBody] AppResourceDto appResource)
        {
            try
            {
                bool success = await repository.MapResource(appResource);
                if (success)
                {
                    return Ok(DataResponse.Succeeded("Mapped app-resource successfully"));
                }
                return NotFound(DataResponse.Failure($"App-resource not found"));
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is ArgumentNullException || ex is ArgumentException)
                {
                    return BadRequest(DataResponse.Failure("Invalid app-resource id"));
                }
                else if (ex is NotFoundException)
                {
                    return NotFound(DataResponse.Failure("App-resource not found"));
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
        public async Task<IActionResult> Unmap([FromBody] AppResourceDto appResource)
        {
            try
            {
                bool success = await repository.UnmapResource(appResource);
                if (success)
                {
                    return Ok(DataResponse.Succeeded("Unmapped app-resource successfully"));
                }
                return NotFound(DataResponse.Failure($"App-resource not found"));
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException || ex is ArgumentNullException || ex is ArgumentException)
                {
                    return BadRequest(DataResponse.Failure("Invalid app-resource id"));
                }
                else if (ex is NotFoundException)
                {
                    return NotFound(DataResponse.Failure("App-resource not found"));
                }
                else
                {
                    return InternalServerError(DataResponse.Failure(ex.Message));
                }
            }
        }
    }
}