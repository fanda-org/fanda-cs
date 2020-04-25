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
    public class OrganizationsController : Controller
    {
        //private const string OrgId = "AAEFE9D0-A36E-46E0-1E19-08D5EA042032";
        private readonly IOrganizationService _service;
        private readonly IAccountYearService _yearService;

        public OrganizationsController(IOrganizationService service, IAccountYearService yearService)
        {
            _service = service;
            _yearService = yearService;
        }

        [HttpGet]
        // GET: Orgs
        public async Task<ActionResult> Index(string search)
        {
            var currentOrg = HttpContext.Session.Get<OrganizationDto>("CurrentOrg");
            var currentYear = HttpContext.Session.Get<AccountYearDto>("CurrentYear");

            OkObjectResult orgResult = (OkObjectResult)await GetAll();
            var orgs = (JsGridResult<IList<OrgYearListDto>>)orgResult.Value;

            foreach (var org in orgs.Data)
            {
                if (currentOrg != null && org.Id == currentOrg.Id)
                {
                    org.IsSelected = true;
                    foreach (var year in org.AccountYears)
                    {
                        if (currentYear != null && year.Id == currentYear.Id)
                        {
                            org.SelectedYearId = currentYear.Id;
                        }
                    }
                }
            }

            ViewBag.Search = search;
            return View(orgs.Data);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                NameValueCollection qFilter = HttpUtility.ParseQueryString(Request.QueryString.Value);
                string search = qFilter["search"];
                var filter = new BaseFilter<IOrganizationService, OrgYearListDto>
                {
                    PageIndex = string.IsNullOrEmpty(qFilter["pageIndex"]) ? 1 : Convert.ToInt32(qFilter["pageIndex"]),
                    PageSize = string.IsNullOrEmpty(qFilter["pageSize"]) ? 100 : Convert.ToInt32(qFilter["pageSize"]),
                    SortField = qFilter["sortField"],
                    SortOrder = qFilter["sortOrder"],
                    Code = string.IsNullOrEmpty(qFilter["code"]) ? search : qFilter["code"],
                    Name = string.IsNullOrEmpty(qFilter["name"]) ? search : qFilter["name"],
                    Description = string.IsNullOrEmpty(qFilter["description"]) ? search : qFilter["description"],
                    Active = string.IsNullOrEmpty(qFilter["active"]) ? (bool?)null : bool.Parse(qFilter["Active"])
                };

                PagedList<OrgYearListDto> data;
                if (string.IsNullOrEmpty(search))
                {
                    data = await filter.ApplyAllAsync(_service);
                }
                else
                {
                    data = await filter.ApplyAnyAsync(_service);
                }

                var result = new JsGridResult<IList<OrgYearListDto>> { Data = data.List, ItemsCount = data.RowCount };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Open([FromForm] OrgYearListDto org)
        {
            if (org.Id != Guid.Empty)
            {
                var orgSelected = await _service.GetByIdAsync(org.Id);
                HttpContext.Session.Set("CurrentOrg", orgSelected);
            }
            if (org.SelectedYearId != Guid.Empty)
            {
                var yearSelected = await _yearService.GetByIdAsync(org.SelectedYearId);
                HttpContext.Session.Set("CurrentYear", yearSelected);
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: Orgs/Create
        public IActionResult Create()
        {
            var org = new OrganizationDto { Active = true };

            //TempData.Set("PartyCategories", await GetPartyCategories(party.CategoryId));
            ViewBag.Mode = "Create";
            ViewBag.Readonly = false;
            return PartialView("_OrganizationEdit", org);   // View("Edit", org);
        }

        // GET: Orgs/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                return NotFound();
            }

            var org = await _service.GetByIdAsync(id);
            if (org == null)
            {
                return NotFound();
            }

            //TempData.Set("PartyCategories", await GetPartyCategories(org.CategoryId));
            ViewBag.Mode = "Edit";
            ViewBag.Readonly = false;
            return PartialView("_OrganizationEdit", org);   // View(org);
        }

        public async Task<JsonResult> GetChildren(Guid id)
        {
            if (id != null && id != Guid.Empty)
            {
                var orgChildren = await _service.GetChildrenByIdAsync(id);
                if (orgChildren != null)
                {
                    return Json(orgChildren);
                }
            }
            return Json(new OrgChildrenDto
            {
                Contacts = new List<ContactDto>(),
                Addresses = new List<AddressDto>()
            });
        }

        // POST: Orgs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromForm] OrganizationDto org)
        {
            try
            {
                #region Validation: Basic model validation
                if (ModelState.IsValid)
                {
                    #region
                    bool isValid = await _service.ValidateAsync(org);
                    if (!isValid)
                    {
                        foreach(var err in org.Errors)
                        {
                            ModelState.AddModelError(err.Key, err.Value);
                        }
                    }
                    #endregion
                    if (ModelState.IsValid)
                    {
                        org = await _service.SaveAsync(org);
                    }
                }
                #endregion

                return PartialView("_OrganizationEdit", org);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    ModelState.AddModelError("Error", ex.InnerException.Message);
                }
                else
                {
                    ModelState.AddModelError("Error", ex.Message);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }
        }

        #region Old actions
        [ValidateAntiForgeryToken]
        [HttpPatch]
        public async Task<IActionResult> ChangeStatus([FromBody] ActiveStatus status)
        {
            await _service.ChangeStatusAsync(status);
            return Ok();
        }

        // GET: Orgs/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                return NotFound();
            }

            var org = await _service.GetByIdAsync(id);
            if (org == null)
            {
                return NotFound();
            }

            ViewBag.Mode = "Details";
            ViewBag.Readonly = true;
            return View("Edit", org);
        }

        // POST: Orgs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OrganizationDto model)
        {
            try
            {
                if (model.Id == null || model.Id == Guid.Empty)
                {
                    ViewBag.Mode = "Create";
                }
                else
                {
                    ViewBag.Mode = "Edit";
                }

                ViewBag.Readonly = false;

                if (!ModelState.IsValid)
                {
                    return PartialView("_orgEdit", model); //View("Edit", model);
                }

                model.Contacts = model.Contacts
                    .Where(c => !c.IsDeleted)
                    .ToList();

                var orgId = HttpContext.Session.Get<OrganizationDto>("CurrentOrg").Id;
                bool create = model.Id == null || model.Id == Guid.Empty;
                await _service.SaveAsync(model);
                if (create) // Create
                {
                    return PartialView("_orgEdit", new OrganizationDto { Active = true });   //RedirectToAction(nameof(Create), new { contactType = model.PartyType });
                }
                else
                {
                    return PartialView("_orgEdit");   //RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    //if ((ex.InnerException as SqlException)?.Number == 2601)
                    //    ModelState.AddModelError("Error", "Code/Name already existst!");
                    //else
                    ModelState.AddModelError("Error", ex.InnerException.Message);
                }
                else
                {
                    ModelState.AddModelError("Error", ex.Message);
                }

                return View("Edit", model);
            }
        }

        // GET: Orgs/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                return NotFound();
            }

            var org = await _service.GetByIdAsync(id);
            if (org == null)
            {
                return NotFound();
            }

            ViewBag.Mode = "Delete";
            ViewBag.Readonly = true;
            return View("Edit", org);
        }

        // POST: Orgs/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteConfirmed(Guid id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return new JsonResult(new
                {
                    ok = true,
                    url = Url.Action(nameof(Index))
                });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);

                ViewBag.Mode = "Delete";
                ViewBag.Readonly = true;
                return new JsonResult(new
                {
                    ok = false,
                    message = ex.Message,
                    url = Url.Action(nameof(Edit))
                });
            }
        }

        //[HttpGet("{id}")]
        ////[ValidateAntiForgeryToken]
        //public IActionResult Select([FromRoute]string id)
        //{
        //    HttpContext.Session.Set("CurrentOrg", id);
        //    return RedirectToAction("Index", "Home");
        //}

        #region Contacts
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddContact(int index)
        {
            var newContact = new ContactDto { Index = index };
            ViewData.TemplateInfo.HtmlFieldPrefix = string.Format("Contacts[{0}]", index);
            return PartialView("~/Views/Shared/EditorTemplates/ContactViewModel.cshtml", newContact);
        }
        #endregion Contacts

        #region Private
        //private async Task<List<SelectListItem>> GetPartyCategories(string currentCategoryId)
        //{
        //    //return await _categoryService.GetAll(OrgId).ToListAsync();
        //    var orgId = HttpContext.Session.Get<OrganizationViewModel>("DemoOrg").OrgId.ToString();
        //    var list = new List<SelectListItem>();
        //    foreach (var category in await _categoryService.GetAll(orgId).ToListAsync())
        //    {
        //        var listItem = new SelectListItem { Value = category.CategoryId, Text = category.Name };
        //        if (!string.IsNullOrEmpty(currentCategoryId) && category.CategoryId == currentCategoryId) listItem.Selected = true;
        //        if (!category.Active) listItem.Disabled = true;

        //        list.Add(listItem);
        //    }
        //    if (string.IsNullOrEmpty(currentCategoryId))
        //    {
        //        var item = list.Find(x => x.Text.Equals("Default", StringComparison.OrdinalIgnoreCase));
        //        if (item != null) item.Selected = true;
        //    }

        //    return list;
        //}
        #endregion Private

        #endregion
    }
}
