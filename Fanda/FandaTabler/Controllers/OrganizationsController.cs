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
    public class OrganizationsController : Controller
    {
        private readonly IOrganizationService _service;
        private readonly IAccountYearService _yearService;

        public OrganizationsController(IOrganizationService service, IAccountYearService yearService)
        {
            _service = service;
            _yearService = yearService;
        }

        // GET: Orgs
        public ActionResult Index() => View();

        //[NonAction]   // [HttpGet]
        //[Produces("application/json")]
        private async Task<JsGridResult<IList<OrgYearListDto>>> GetAll()
        {
            try
            {
                NameValueCollection qFilter = HttpUtility.ParseQueryString(Request.QueryString.Value);
                string search = qFilter["search"];

                var filter = new Filter<IOrganizationService, OrgYearListDto>(_service, qFilter, search);
                var data = await filter.ApplyAsync();
                var result = new JsGridResult<IList<OrgYearListDto>>
                {
                    Data = data.List,
                    ItemsCount = data.ItemsCount,
                    Page = data.Page,
                    PageCount = data.PageCount,
                    FirstRowOnPage = data.FirstRowOnPage,
                    LastRowOnPage = data.LastRowOnPage
                };

                return result;
            }
            catch (Exception ex)
            {
                var result = new JsGridResult<IList<OrgYearListDto>> { Error = ex.Message };
                return result;
            }
        }

        // GET: Orgs/List
        public async Task<IActionResult> List(string search = "" /*, int pageIndex = 1, int pageSize = 100*/)
        {
            ViewBag.Search = search;
            var currentOrg = HttpContext.Session.Get<OrganizationDto>("CurrentOrg");
            var currentYear = HttpContext.Session.Get<AccountYearDto>("CurrentYear");

            var orgs = await GetAll();
            #region Selected org and accounting year
            if (orgs.Data != null)
            {
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
            }
            #endregion

            return PartialView("_OrganizationList", orgs);
        }

        // POST: Orgs/Open
        [HttpPost]
        [ValidateAntiForgeryToken]
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
            //ViewBag.Mode = "Create";
            //ViewBag.Readonly = false;
            return PartialView("_OrganizationEdit", org);   // View("Edit", org);
        }

        // GET: Orgs/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                return BadRequest();
            }
            var org = await _service.GetByIdAsync(id);
            if (org == null)
            {
                return NotFound();
            }
            //ViewBag.Mode = "Edit";
            //ViewBag.Readonly = false;
            return PartialView("_OrganizationEdit", org);   // View(org);
        }

        // GET: Orgs/GetChildren/5
        public async Task<IActionResult> GetChildren(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                return BadRequest();
            }
            var orgChildren = await _service.GetChildrenByIdAsync(id);
            if (orgChildren == null)
            {
                return NotFound();
            }

            return Ok(orgChildren);
        }

        // POST: Orgs/Save
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
                        foreach (var err in org.Errors)
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
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: Orgs/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                return BadRequest();
            }
            var result = await _service.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPatch]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus([FromBody] ActiveStatus status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _service.ChangeStatusAsync(status);
            return Ok();
        }

        #region Old actions        
        // GET: Orgs/Details/5
        //public async Task<IActionResult> Details(Guid id)
        //{
        //    if (id == null || id == Guid.Empty)
        //    {
        //        return NotFound();
        //    }

        //    var org = await _service.GetByIdAsync(id);
        //    if (org == null)
        //    {
        //        return NotFound();
        //    }

        //    ViewBag.Mode = "Details";
        //    ViewBag.Readonly = true;
        //    return View("Edit", org);
        //}

        // POST: Orgs/Edit/5

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(OrganizationDto model)
        //{
        //    try
        //    {
        //        if (model.Id == null || model.Id == Guid.Empty)
        //        {
        //            ViewBag.Mode = "Create";
        //        }
        //        else
        //        {
        //            ViewBag.Mode = "Edit";
        //        }

        //        ViewBag.Readonly = false;

        //        if (!ModelState.IsValid)
        //        {
        //            return PartialView("_orgEdit", model); //View("Edit", model);
        //        }

        //        model.Contacts = model.Contacts
        //            .Where(c => !c.IsDeleted)
        //            .ToList();

        //        var orgId = HttpContext.Session.Get<OrganizationDto>("CurrentOrg").Id;
        //        bool create = model.Id == null || model.Id == Guid.Empty;
        //        await _service.SaveAsync(model);
        //        if (create) // Create
        //        {
        //            return PartialView("_orgEdit", new OrganizationDto { Active = true });   //RedirectToAction(nameof(Create), new { contactType = model.PartyType });
        //        }
        //        else
        //        {
        //            return PartialView("_orgEdit");   //RedirectToAction(nameof(Index));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.InnerException != null)
        //        {
        //            //if ((ex.InnerException as SqlException)?.Number == 2601)
        //            //    ModelState.AddModelError("Error", "Code/Name already existst!");
        //            //else
        //            ModelState.AddModelError("Error", ex.InnerException.Message);
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("Error", ex.Message);
        //        }

        //        return View("Edit", model);
        //    }
        //}

        // GET: Orgs/Delete/5

        //public async Task<IActionResult> DeleteGet(Guid id)
        //{
        //    if (id == null || id == Guid.Empty)
        //    {
        //        return NotFound();
        //    }

        //    var org = await _service.GetByIdAsync(id);
        //    if (org == null)
        //    {
        //        return NotFound();
        //    }

        //    ViewBag.Mode = "Delete";
        //    ViewBag.Readonly = true;
        //    return View("Edit", org);
        //}

        //[HttpGet("{id}")]
        ////[ValidateAntiForgeryToken]
        //public IActionResult Select([FromRoute]string id)
        //{
        //    HttpContext.Session.Set("CurrentOrg", id);
        //    return RedirectToAction("Index", "Home");
        //}

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
