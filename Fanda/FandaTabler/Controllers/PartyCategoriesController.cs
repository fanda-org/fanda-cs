using Fanda.Dto;
using Fanda.Service;
using Fanda.Service.Base;
using Fanda.Shared;
using FandaTabler.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web;

namespace FandaTabler.Controllers
{
    [Authorize]
    public class PartyCategoriesController : Controller
    {
        private readonly IPartyCategoryService _service;

        public PartyCategoriesController(IPartyCategoryService service)
        {
            _service = service;
        }

        public ActionResult IndexEdit() => View();

        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult> GetAll(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    var org = GetSelectedOrg();
                    if (org == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    id = org.Id;
                }

                NameValueCollection qFilter = HttpUtility.ParseQueryString(Request.QueryString.Value);
                string search = qFilter["search"];

                var filter = new OrgFilter<IPartyCategoryService, PartyCategoryListDto>(_service, qFilter, search);
                var data = await filter.ApplyAsync(id);
                var result = new JsGridResult<IList<PartyCategoryListDto>> { Data = data.List, ItemsCount = data.ItemsCount };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(PartyCategoryDto model)
        {
            try
            {
                var org = GetSelectedOrg();
                if (org == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                #region Validation
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                model.Code = model.Code.ToUpper();
                model.Name = model.Name.TrimExtraSpaces();
                model.Description = model.Description.TrimExtraSpaces();
                #region Validation: Dupllicate
                // Check code duplicate
                var duplCode = new BaseOrgDuplicate { Field = DuplicateField.Code, Value = model.Code, Id = model.Id, OrgId = org.Id };
                if (await _service.ExistsAsync(duplCode))
                {
                    ModelState.AddModelError("Code", $"Code '{model.Code}' already exists");
                }
                // Check name duplicate
                var duplName = new BaseOrgDuplicate { Field = DuplicateField.Name, Value = model.Name, Id = model.Id, OrgId = org.Id };
                if (await _service.ExistsAsync(duplName))
                {
                    ModelState.AddModelError("Name", $"Name '{model.Name}' already exists");
                }
                #endregion

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                #endregion

                model = await _service.SaveAsync(org.Id, model);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await _service.DeleteAsync(new Guid(id));
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
            }
        }

        private OrganizationDto GetSelectedOrg() => HttpContext.Session.Get<OrganizationDto>("CurrentOrg");
    }
}