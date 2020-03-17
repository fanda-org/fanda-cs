using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DataTables.Queryable;
using FandaNg.Extensions;
using FandaNg.Models;
using Fanda.Service.Business;
using Fanda.ViewModel.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Sieve.Models;
using Sieve.Services;

namespace FandaNg.Controllers
{
    [Route("api/[controller]")]
    public class PartyCategoriesController : Controller
    {
        //private const string OrgId = "AAEFE9D0-A36E-46E0-1E19-08D5EA042032";
        private readonly IPartyCategoryService _service;

        public PartyCategoriesController(IPartyCategoryService service)
        {
            _service = service;
        }

        // GET: PartyCategories
        public ActionResult Index()
        {
            return View();
        }

        [Produces("application/json")]
        public async Task<JsonResult> GetAll()
        {
            var orgId = HttpContext.Session.Get<OrganizationViewModel>("SampleOrg").OrgId.ToString();
            var request = new DataTablesRequest<PartyCategoryViewModel>(
                Request.QueryString.Value
                );
            var result = await _service
                .GetAll(orgId)
                .ToPagedListAsync(request);
            return result.JsonDataTable(request.Draw);
        }

        [ValidateAntiForgeryToken]
        [HttpPatch]
        public async Task<IActionResult> ChangeStatus([FromBody]ActiveStatus status)
        {
            await _service.ChangeStatus(status.Id, status.Active);
            return Ok();
        }

        // GET: PartyCategories/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var cat = await _service.GetByIdAsync(id);
            if (cat == null)
                return NotFound();

            ViewBag.Mode = "Details";
            ViewBag.Readonly = true;
            return View("Edit", cat);
        }

        // GET: PartyCategories/Create
        public ActionResult Create()
        {
            var cat = new PartyCategoryViewModel { Active = true };
            ViewBag.Mode = "Create";
            ViewBag.Readonly = false;
            return View("Edit", cat);
        }

        // GET: PartyCategories/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var cat = await _service.GetByIdAsync(id);
            if (cat == null)
                return NotFound();

            ViewBag.Mode = "Edit";
            ViewBag.Readonly = false;
            return View(cat);
        }

        // POST: PartyCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PartyCategoryViewModel model)
        {
            try
            {
                var orgId = HttpContext.Session.Get<OrganizationViewModel>("SampleOrg").OrgId.ToString();
                bool create = string.IsNullOrEmpty(model.CategoryId);
                await _service.SaveAsync(orgId, model);
                if (create) // Create
                    return RedirectToAction(nameof(Create));
                else
                    return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    if ((ex.InnerException as SqlException)?.Number == 2601)
                        ModelState.AddModelError("Error", "Code/Name already existst!");
                    else
                        ModelState.AddModelError("Error", ex.InnerException.Message);
                else
                    ModelState.AddModelError("Error", ex.Message);

                if (string.IsNullOrEmpty(model.CategoryId))
                    ViewBag.Mode = "Create";
                else
                    ViewBag.Mode = "Edit";
                ViewBag.Readonly = false;
                return View("Edit", model);
            }
        }

        [HttpGet]
        // GET: PartyCategories/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var cat = await _service.GetByIdAsync(id);
            if (cat == null)
                return NotFound();

            ViewBag.Mode = "Delete";
            ViewBag.Readonly = true;
            return View("Edit", cat);
        }

        // POST: PartyCategories/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
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
    }
}