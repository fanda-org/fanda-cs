using Fanda.Dto;
using Fanda.Service;
using Fanda.Shared;
using FandaTabler.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
                var filter = new
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

                var query = _service.GetAll(org.Id)
                    .Where(c =>
                        (string.IsNullOrEmpty(filter.Code) || c.Code.Contains(filter.Code))
                        && (string.IsNullOrEmpty(filter.Name) || c.Name.Contains(filter.Name))
                        && (string.IsNullOrEmpty(filter.Description) || c.Description.Contains(filter.Description))
                        && (!filter.Active.HasValue || c.Active == filter.Active)
                    );

                if (filter.SortField != null && filter.SortOrder != null)
                {
                    query = query.OrderBy($"{filter.SortField} {filter.SortOrder}");
                }
                else
                {
                    query = query.OrderBy("Code asc");
                }
                var data = await query.GetPagedAsync(filter.PageIndex, filter.PageSize);
                var result = new { data = data.List, itemsCount = data.RowCount };
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
                var duplCode = new Duplicate { Field = DuplicateField.Code, Value = model.Code, Id = model.Id, OrgId = org.Id };
                if (await _service.ExistsAsync(duplCode))
                {
                    ModelState.AddModelError("Code", $"Code '{model.Code}' already exists");
                }
                // Check name duplicate
                var duplName = new Duplicate { Field = DuplicateField.Name, Value = model.Name, Id = model.Id, OrgId = org.Id };
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

        private OrganizationDto GetSelectedOrg()
        {
            return HttpContext.Session.Get<OrganizationDto>("CurrentOrg");
        }
    }
}