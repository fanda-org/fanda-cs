using Fanda.Dto;
using Fanda.Service;
using FandaTabler.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FandaTabler.Controllers
{
    //[Route("api/[controller]")]
    [Authorize]
    public class PartyCategoriesController : Controller
    {
        //private const string OrgId = "AAEFE9D0-A36E-46E0-1E19-08D5EA042032";
        private readonly IPartyCategoryService _service;

        public PartyCategoriesController(IPartyCategoryService service)
        {
            _service = service;
        }

        public ActionResult IndexEdit() => View();

        //// GET: PartyCategories
        //public ActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        [Produces("application/json")]
        public async Task<JsonResult> GetAll()
        {
            NameValueCollection qFilter = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var filter = new
            {
                Code = qFilter["code"],
                Name = qFilter["name"],
                Description = qFilter["description"],
                Active = string.IsNullOrEmpty(qFilter["active"]) ? (bool?)null : bool.Parse(qFilter["Active"])
                //(qFilter["Country"] == "0") ? (Country?)null : (Country)int.Parse(qFilter["Country"]),
                //Married = string.IsNullOrEmpty(qFilter["Married"]) ? (bool?)null : bool.Parse(qFilter["Married"])
            };

            var org = HttpContext.Session.Get<OrganizationDto>("CurrentOrg");
            if (org != null)
            {
                var data = await _service.GetAll(org.OrgId)
                    .Where(c =>
                        (string.IsNullOrEmpty(filter.Code) || c.Code.ToLower().Contains(filter.Code.ToLower())) &&
                        (string.IsNullOrEmpty(filter.Name) || c.Name.ToLower().Contains(filter.Name.ToLower())) &&
                        (string.IsNullOrEmpty(filter.Description) || c.Description.ToLower().Contains(filter.Description.ToLower())) &&
                        (!filter.Active.HasValue || c.Active == filter.Active)
                    )
                    .ToListAsync();
                return Json(data);
            }
            else
            {
                return null;
            }
            //var orgId = HttpContext.Session.Get<OrganizationViewModel>("DemoOrg").OrgId.ToString();
            //var request = new DataTablesRequest<PartyCategoryViewModel>(
            //    Request.QueryString.Value
            //    );
            //var result = await _service
            //    .GetAll(orgId)
            //    .ToPagedListAsync(request);
            //return result.JsonDataTable(request.Draw);
        }

        //[ValidateAntiForgeryToken]
        //[HttpPatch]
        //public async Task<IActionResult> ChangeStatus([FromBody]ActiveStatus status)
        //{
        //    await _service.ChangeStatus(status.Id, status.Active);
        //    return Ok();
        //}

        //// GET: PartyCategories/Details/5
        //public async Task<ActionResult> Details(string id)
        //{
        //    if (string.IsNullOrEmpty(id))
        //        return NotFound();

        //    var cat = await _service.GetByIdAsync(id);
        //    if (cat == null)
        //        return NotFound();

        //    ViewBag.Mode = "Details";
        //    ViewBag.Readonly = true;
        //    return View("Edit", cat);
        //}

        //// GET: PartyCategories/Create
        //public ActionResult Create()
        //{
        //    var cat = new PartyCategoryViewModel { Active = true };
        //    ViewBag.Mode = "Create";
        //    ViewBag.Readonly = false;
        //    return View("Edit", cat);
        //}

        //// GET: PartyCategories/Edit/5
        //public async Task<ActionResult> Edit(string id)
        //{
        //    if (string.IsNullOrEmpty(id))
        //        return NotFound();

        //    var cat = await _service.GetByIdAsync(id);
        //    if (cat == null)
        //        return NotFound();

        //    ViewBag.Mode = "Edit";
        //    ViewBag.Readonly = false;
        //    return View(cat);
        //}

        // POST: PartyCategories/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(PartyCategoryDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                var orgId = HttpContext.Session.Get<OrganizationDto>("CurrentOrg").OrgId;
                model = await _service.SaveAsync(orgId, model);
                return Ok(model); //Json(new { Success = true, Message = string.Empty }/*, JsonRequestBehavior.AllowGet*/);
            }
            catch (Exception ex)
            {
                //return Json(new { Success = false, Message = ex.InnerMessage() });
                //throw ex;
                return StatusCode(403, Json(ex.Message));
            }
        }

        //[HttpGet]
        //// GET: PartyCategories/Delete/5
        //public async Task<ActionResult> Delete(string id)
        //{
        //    if (string.IsNullOrEmpty(id))
        //        return NotFound();

        //    var cat = await _service.GetByIdAsync(id);
        //    if (cat == null)
        //        return NotFound();

        //    ViewBag.Mode = "Delete";
        //    ViewBag.Readonly = true;
        //    return View("Edit", cat);
        //}

        // POST: PartyCategories/Delete/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(PartyDto model)
        {
            //try
            //{
            await _service.DeleteAsync(model.CategoryId);
            return Ok();
            //return new JsonResult(new
            //{
            //    ok = true,
            //    url = Url.Action(nameof(Index))
            //});
            //}
            //catch (Exception ex)
            //{
            //    ModelState.AddModelError("Error", ex.Message);
            //    ViewBag.Mode = "Delete";
            //    ViewBag.Readonly = true;
            //    return new JsonResult(new
            //    {
            //        ok = false,
            //        message = ex.Message,
            //        url = Url.Action(nameof(Edit))
            //    });
            //}
        }
    }
}