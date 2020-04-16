using Fanda.Dto;
using Fanda.Service;
using Fanda.Shared;
using FandaTabler.Extensions;
using FandaTabler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Dynamic.Core;
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
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var org = GetSelectedOrg();
                if (org == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                NameValueCollection qFilter = HttpUtility.ParseQueryString(Request.QueryString.Value);
                var filter = new BaseOrgFilter<IPartyCategoryService, PartyCategoryDto>
                {
                    PageIndex = Convert.ToInt32(qFilter["pageIndex"]),
                    PageSize = Convert.ToInt32(qFilter["pageSize"]),
                    SortField = qFilter["sortField"],
                    SortOrder = qFilter["sortOrder"],
                    Code = qFilter["code"],
                    Name = qFilter["name"],
                    Description = qFilter["description"],
                    Active = string.IsNullOrEmpty(qFilter["active"]) ? (bool?)null : bool.Parse(qFilter["Active"])
                    //(qFilter["Country"] == "0") ? (Country?)null : (Country)int.Parse(qFilter["Country"]),
                    //Married = string.IsNullOrEmpty(qFilter["Married"]) ? (bool?)null : bool.Parse(qFilter["Married"])
                };

                var data = await filter.ApplyAsync(_service, org.Id);
                var result = new JsGridResult<IList<PartyCategoryDto>>{ Data = data.List, ItemsCount = data.RowCount };
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