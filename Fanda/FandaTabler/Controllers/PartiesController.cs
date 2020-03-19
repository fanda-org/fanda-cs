//using Fanda.Common.Models.DataTables;
using DataTables.Queryable;
using Fanda.Dto;
using Fanda.Service;
using Fanda.Shared;
using FandaTabler.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FandaTabler.Controllers
{
    //[Route("api/[controller]")]
    [Authorize]
    public class PartiesController : Controller
    {
        //private const string OrgId = "AAEFE9D0-A36E-46E0-1E19-08D5EA042032";
        private readonly IPartyService _service;
        private readonly IPartyCategoryService _categoryService;

        public PartiesController(IPartyService service, IPartyCategoryService categoryService)
        {
            _service = service;
            _categoryService = categoryService;
        }

        // GET: Parties
        public ActionResult Index()
        {
            TempData.Remove("PartyCategories");
            return View();
        }

        [Produces("application/json")]
        public async Task<JsonResult> GetAll(/*DataTablesParams param*/)
        {
            //NameValueCollection qFilter = HttpUtility.ParseQueryString(Request.QueryString.Value);
            //var filter = new
            //{
            //    Code = qFilter["code"],
            //    Name = qFilter["name"],
            //    Description = qFilter["description"],
            //    Active = string.IsNullOrEmpty(qFilter["active"]) ? (bool?)null : bool.Parse(qFilter["Active"])
            //    //(qFilter["Country"] == "0") ? (Country?)null : (Country)int.Parse(qFilter["Country"]),
            //    //Married = string.IsNullOrEmpty(qFilter["Married"]) ? (bool?)null : bool.Parse(qFilter["Married"])
            //};
            var org = HttpContext.Session.Get<OrganizationDto>("CurrentOrg");
            if (org != null)
            {
                var request = new DataTablesRequest<PartyDto>(
                    Request.QueryString.Value
                    );
                var result = await _service
                    .GetAll(org.OrgId)
                    .ToPagedListAsync(request);
                return result.JsonDataTable(request.Draw);
            }
            else
                return null;
        }

        [ValidateAntiForgeryToken]
        [HttpPatch]
        public async Task<IActionResult> ChangeStatus([FromBody]ActiveStatus status)
        {
            await _service.ChangeStatus(status.Id, status.Active);
            return Ok();
        }

        // GET: Parties/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var party = await _service.GetByIdAsync(id);
            if (party == null)
                return NotFound();

            ViewBag.Mode = "Details";
            ViewBag.Readonly = true;
            return View("Edit", party);
        }

        // GET: Parties/Create
        public async Task<ActionResult> Create(string contactType)
        {
            var party = new PartyDto { PartyType = (PartyType)Enum.Parse(typeof(PartyType), contactType, true), Active = true };

            TempData.Set("PartyCategories", await GetPartyCategories(party.CategoryId));
            ViewBag.Mode = "Create";
            ViewBag.Readonly = false;
            return View("Edit", party);
        }

        // GET: Parties/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var party = await _service.GetByIdAsync(id);
            if (party == null)
                return NotFound();

            TempData.Set("PartyCategories", await GetPartyCategories(party.CategoryId));
            ViewBag.Mode = "Edit";
            ViewBag.Readonly = false;
            return View(party);
        }

        // POST: Parties/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PartyDto model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.PartyId))
                    ViewBag.Mode = "Create";
                else
                    ViewBag.Mode = "Edit";

                ViewBag.Readonly = false;

                if (!ModelState.IsValid)
                    return PartialView("_partyEdit", model); //View("Edit", model);

                model.Contacts = model.Contacts
                    .Where(c => !c.IsDeleted)
                    .ToList();

                var orgId = HttpContext.Session.Get<OrganizationDto>("CurrentOrg").OrgId.ToString();
                bool create = string.IsNullOrEmpty(model.PartyId);
                await _service.SaveAsync(orgId, model);
                if (create) // Create
                    return RedirectToAction(nameof(Create), new { contactType = model.PartyType }); //PartialView("_partyEdit", new PartyViewModel { PartyType = model.PartyType, Active = true });
                else
                    return RedirectToAction(nameof(Index)); //PartialView("_partyEdit");
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

                return View(nameof(Edit), model);
            }
        }

        // GET: Parties/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var party = await _service.GetByIdAsync(id);
            if (party == null)
                return NotFound();

            //TempData.Put("PartyCategories", await GetPartyCategories(party.CategoryId));
            ViewBag.Mode = "Delete";
            ViewBag.Readonly = true;
            return View("Edit", party);
        }

        // POST: Parties/Delete/5
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

        #region Contacts

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddContact(int index)
        {
            var newContact = new ContactDto { Index = index };
            ViewData.TemplateInfo.HtmlFieldPrefix = string.Format("Contacts[{0}]", index);
            return PartialView("~/Views/Shared/EditorTemplates/ContactViewModel.cshtml", newContact);
        }

        #endregion Contacts

        #region Private

        private async Task<List<SelectListItem>> GetPartyCategories(string currentCategoryId)
        {
            //return await _categoryService.GetAll(OrgId).ToListAsync();
            var orgId = HttpContext.Session.Get<OrganizationDto>("CurrentOrg").OrgId.ToString();
            var list = new List<SelectListItem>();
            foreach (var category in await _categoryService.GetAll(orgId).ToListAsync())
            {
                var listItem = new SelectListItem { Value = category.CategoryId, Text = category.Name };
                if (!string.IsNullOrEmpty(currentCategoryId) && category.CategoryId == currentCategoryId) listItem.Selected = true;
                if (!category.Active) listItem.Disabled = true;

                list.Add(listItem);
            }
            if (string.IsNullOrEmpty(currentCategoryId))
            {
                var item = list.Find(x => x.Text.Equals("Default", StringComparison.OrdinalIgnoreCase));
                if (item != null) item.Selected = true;
            }

            return list;
        }

        //private List<SelectListItem> GetPartyCategories(string currentCategoryId)
        //{
        //    var list = new List<SelectListItem>();
        //    foreach (var category in _categoryService.GetAll(OrgId))
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

        //private List<SelectListItem> GetPartyTypes(PartyType currentType)
        //{
        //    var list = new List<SelectListItem>();
        //    foreach (var partyType in FandaEnums.GetPartyTypes())
        //    {
        //        var listItem = new SelectListItem { Value = ((int)partyType).ToString(), Text = partyType.ToString() };
        //        if (partyType == currentType) listItem.Selected = true;

        //        list.Add(listItem);
        //    }
        //    return list;
        //}

        //private List<SelectListItem> GetPaymentTerms(PaymentTerm currentTerm)
        //{
        //    var list = new List<SelectListItem>();
        //    foreach (var paymentTerm in FandaEnums.GetPaymentTerms())
        //    {
        //        var listItem = new SelectListItem { Value = ((int)paymentTerm).ToString(), Text = paymentTerm.ToString() };
        //        if (paymentTerm == currentTerm) listItem.Selected = true;

        //        list.Add(listItem);
        //    }
        //    return list;
        //}

        #endregion Private
    }
}