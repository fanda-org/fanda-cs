using Fanda.Dto;
using Fanda.Service;
using Fanda.Shared.Models;
using FandaTabler.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FandaTabler.Controllers
{
    [Authorize]
    public class OrganizationsController : Controller
    {
        //private const string OrgId = "AAEFE9D0-A36E-46E0-1E19-08D5EA042032";
        private readonly IOrganizationService _service;

        public OrganizationsController(IOrganizationService service)
        {
            _service = service;
        }

        // GET: Orgs
        public ActionResult Index() => View();

        [Produces("application/json")]
        public async Task<JsonResult> GetAll()
        {
            //var request = new DataTablesRequest<OrganizationViewModel>(
            //    Request.QueryString.Value
            //    );
            //var result = await _service
            //    .GetAll()
            //    .ToPagedListAsync(request);
            //return result.JsonDataTable(request.Draw);
            NameValueCollection qFilter = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var filter = new
            {
                Code = qFilter["orgCode"],
                Name = qFilter["orgName"],
                Description = qFilter["description"],
                Active = string.IsNullOrEmpty(qFilter["active"]) ? (bool?)null : bool.Parse(qFilter["Active"])
                //(qFilter["Country"] == "0") ? (Country?)null : (Country)int.Parse(qFilter["Country"]),
                //Married = string.IsNullOrEmpty(qFilter["Married"]) ? (bool?)null : bool.Parse(qFilter["Married"])
            };

            var data = await _service.GetAll()
                .Where(c =>
                    (string.IsNullOrEmpty(filter.Code) || c.OrgCode.ToLower().Contains(filter.Code.ToLower())) &&
                    (string.IsNullOrEmpty(filter.Name) || c.OrgName.ToLower().Contains(filter.Name.ToLower())) &&
                    (string.IsNullOrEmpty(filter.Description) || c.Description.ToLower().Contains(filter.Description.ToLower())) &&
                    (!filter.Active.HasValue || c.Active == filter.Active)
                )
                .ToListAsync();
            return Json(data);
        }

        //[ValidateAntiForgeryToken]
        [HttpGet("{id}")]
        public async Task<IActionResult> Select([FromRoute]string id)
        {
            // ** TODO **
            var org = await _service.GetByIdAsync(id/*, true*/);
            HttpContext.Session.Set("CurrentOrg", org);
            return RedirectToAction("Index", "Home");
        }

        [ValidateAntiForgeryToken]
        [HttpPatch]
        public async Task<IActionResult> ChangeStatus([FromBody]ActiveStatus status)
        {
            await _service.ChangeStatus(status.Id, status.Active);
            return Ok();
        }

        // GET: Orgs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var org = await _service.GetByIdAsync(id);
            if (org == null)
                return NotFound();

            ViewBag.Mode = "Details";
            ViewBag.Readonly = true;
            return View("Edit", org);
        }

        // GET: Orgs/Create
        public IActionResult Create()
        {
            var org = new OrganizationDto { Active = true };

            //TempData.Set("PartyCategories", await GetPartyCategories(party.CategoryId));
            ViewBag.Mode = "Create";
            ViewBag.Readonly = false;
            return View("Edit", org);
        }

        // GET: Orgs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var org = await _service.GetByIdAsync(id);
            if (org == null)
                return NotFound();

            //TempData.Set("PartyCategories", await GetPartyCategories(org.CategoryId));
            ViewBag.Mode = "Edit";
            ViewBag.Readonly = false;
            return View(org);
        }

        // POST: Orgs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OrganizationDto model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.OrgId))
                    ViewBag.Mode = "Create";
                else
                    ViewBag.Mode = "Edit";

                ViewBag.Readonly = false;

                if (!ModelState.IsValid)
                    return PartialView("_orgEdit", model); //View("Edit", model);

                model.Contacts = model.Contacts
                    .Where(c => !c.IsDeleted)
                    .ToList();

                var orgId = HttpContext.Session.Get<OrganizationDto>("CurrentOrg").OrgId;
                bool create = string.IsNullOrEmpty(model.OrgId);
                await _service.SaveAsync(model);
                if (create) // Create
                    return PartialView("_orgEdit", new OrganizationDto { Active = true });   //RedirectToAction(nameof(Create), new { contactType = model.PartyType });
                else
                    return PartialView("_orgEdit");   //RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    //if ((ex.InnerException as SqlException)?.Number == 2601)
                    //    ModelState.AddModelError("Error", "Code/Name already existst!");
                    //else
                    ModelState.AddModelError("Error", ex.InnerException.Message);
                else
                    ModelState.AddModelError("Error", ex.Message);

                return View("Edit", model);
            }
        }

        // GET: Orgs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var org = await _service.GetByIdAsync(id);
            if (org == null)
                return NotFound();

            ViewBag.Mode = "Delete";
            ViewBag.Readonly = true;
            return View("Edit", org);
        }

        // POST: Orgs/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteConfirmed(string id)
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
    }
}
