using Fanda.Dto;
using Fanda.Repository;
using Fanda.Repository.Base;
using Fanda.Repository.Extensions;
using FandaTabler.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Specialized;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace FandaTabler.Controllers
{
    [Authorize]
    public class OrganizationsController : Controller
    {
        private readonly IOrganizationRepository _repository;
        private readonly IAccountYearRepository _yearRepository;

        public OrganizationsController(IOrganizationRepository repository, IAccountYearRepository yearRepository)
        {
            _repository = repository;
            _yearRepository = yearRepository;
        }

        // GET: Orgs
        public ActionResult Index() => View();

        // GET: Orgs/List
        public async Task<IActionResult> List(/*string search = "", int pageIndex = 1, int pageSize = 100*/)
        {
            try
            {
                //ViewBag.Search = search;
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
                return PartialView("_List", orgs);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                ViewBag.Error = errorMessage;
                return PartialView("_List", null);
            }
        }

        // POST: Orgs/Open
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Open([FromForm] OrgYearListDto org)
        {
            await Open(org.Id, org.SelectedYearId);
            return RedirectToAction("Index", "Home");
        }

        // GET: Orgs/Create
        public IActionResult Create()
        {
            var org = new OrganizationDto { Active = true };
            return PartialView("_Edit", org);
        }

        // GET: Orgs/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                if (id == null || id == Guid.Empty)
                {
                    return BadRequest(new { Error = "Id is missing" });
                }
                var org = await _repository.GetByIdAsync(id);
                if (org == null)
                {
                    return NotFound(new { Error = "Organization not found" });
                }
                return PartialView("_Edit", org);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                ViewBag.Error = errorMessage;
                return PartialView("_Edit", null);
            }
        }

        // GET: Orgs/GetChildren/5
        public async Task<IActionResult> GetChildren(Guid id)
        {
            try
            {
                if (id == null || id == Guid.Empty)
                {
                    return BadRequest();
                }
                var orgChildren = await _repository.GetChildrenByIdAsync(id);
                if (orgChildren == null)
                {
                    return NotFound();
                }

                return Ok(orgChildren);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = errorMessage });
            }
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
                    var errors = await _repository.ValidateAsync(org);
                    foreach (var err in errors)
                    {
                        ModelState.AddModelError(err.Key, err.Value);
                    }
                    #endregion
                    if (ModelState.IsValid)
                    {
                        bool isAdding = org.Id == null || org.Id == Guid.Empty;

                        org = await _repository.SaveAsync(org);

                        if (isAdding)
                        {
                            Guid userId = GetCurrentUserId();
                            await _repository.MapUserAsync(org.Id, userId);
                        }

                        // Refresh org session value
                        // TODO: for accounting year
                        var orgOpened = HttpContext.Session.Get<OrganizationDto>("CurrentOrg");
                        if (org.Id == orgOpened?.Id)
                        {
                            await Open(org.Id, Guid.Empty);
                        }
                    }
                }
                #endregion

                return PartialView("_Edit", org);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                ViewBag.Error = errorMessage;
                return PartialView("_Edit", org);
            }
        }

        // POST: Orgs/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                if (id == null || id == Guid.Empty)
                {
                    return BadRequest();
                }
                var result = await _repository.DeleteAsync(id);
                if (!result)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = errorMessage });
            }
        }

        #region Private methods
        private async Task<PagedResponse<OrgYearListDto>> GetAll()
        {
            Guid userId = GetCurrentUserId();

            NameValueCollection qFilter = HttpUtility.ParseQueryString(Request.QueryString.Value);
            //string search = qFilter["search"];
            //var filter = new ChildFilter<IOrganizationRepository, OrgYearListDto>(_repository, qFilter, search);
            //var result = await filter.ApplyAsync(userId);
            var response = await _repository
                .GetPaged(userId, new Query { 
                    Page = Convert.ToInt32(qFilter["pageIndex"]),
                    PageSize = Convert.ToInt32(qFilter["pageSize"]),
                    Search = qFilter["search"]
                });
            return response;
        }

        private Guid GetCurrentUserId()
        {
            string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            Guid userGuid = new Guid(userId);
            if (userGuid == null || userGuid == Guid.Empty)
            {
                throw new ApplicationException("UserId not found in logged in user identity");
            }

            return userGuid;
        }

        private async Task Open(Guid orgId, Guid yearId)
        {
            #region Open org
            if (orgId != Guid.Empty)
            {
                var orgSelected = await _repository.GetByIdAsync(orgId);
                if (orgSelected.Active)
                {
                    HttpContext.Session.Set("CurrentOrg", orgSelected);
                }
                else
                {
                    HttpContext.Session.Remove("CurrentOrg");
                }
            }
            else
            {
                HttpContext.Session.Remove("CurrentOrg");
            }
            #endregion
            #region Open year
            if (yearId != Guid.Empty)
            {
                var yearSelected = await _yearRepository.GetByIdAsync(yearId);
                if (yearSelected.Active)
                {
                    HttpContext.Session.Set("CurrentYear", yearSelected);
                }
                else
                {
                    HttpContext.Session.Remove("CurrentYear");
                }
            }
            else
            {
                HttpContext.Session.Remove("CurrentYear");
            }
            #endregion
        }
        #endregion

        #region Old actions
        //[HttpPatch]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ChangeStatus([FromBody] ActiveStatus status)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }

        //    await _service.ChangeStatusAsync(status);
        //    return Ok();
        //}

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
