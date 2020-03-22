using DataTables.Queryable;
using Fanda.Dto;
using Fanda.Service;
using Fanda.Shared;
using FandaCoreUI.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
namespace Fanda.Controllers
{
    public class PartyCategoriesController : Controller
    {
        //private const string OrgId = "AAEFE9D0-A36E-46E0-1E19-08D5EA042032";
        private readonly IPartyCategoryService _service;

        public PartyCategoriesController(IPartyCategoryService service)
        {
            _service = service;
        }

        // GET: PartyCategories
        public ActionResult Index() => View();

        [Produces("application/json")]
        public async Task<JsonResult> GetAll()
        {
            var org = HttpContext.Session.Get<OrganizationDto>("DemoOrg");
            if (org == null)
                return null;

            var request = new DataTablesRequest<PartyCategoryDto>(
                Request.QueryString.Value
                );
            var result = await _service
                .GetAll(org.Id)
                .ToPagedListAsync(request);
            //var dt = JsonDataTable(result, request.Draw); //result.JsonDataTable(request.Draw);
            //return dt;
            var dt = result.JsonDataTable(request.Draw);
            return dt;
        }

        /// <summary>
        /// Helper method that converts <see cref="IPagedList{T}"/> collection to the JSON-serialized object in datatables-friendly format.
        /// </summary>
        /// <param name="model"><see cref="IPagedList{T}"/> collection of items</param>
        /// <param name="draw">Draw counter (optional).</param>
        /// <returns>JsonNetResult instance to be sent to datatables</returns>
        //protected JsonNetResult JsonDataTable<T>(IPagedList<T> model, int draw = 0)
        //{
        //    JsonNetResult jsonResult = new JsonNetResult();
        //    //jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        //    jsonResult.Data = new
        //    {
        //        draw = draw,
        //        recordsTotal = model.TotalCount,
        //        recordsFiltered = model.TotalCount,
        //        data = model
        //    };
        //    return jsonResult;
        //}

        /// <summary>
        /// JsonNet-based JsonResult
        /// </summary>
        //protected class JsonNetResult : JsonResult
        //{
        //    public object Data { get; set; }
        //    public JsonNetResult() : base(null) { }

        //    public override async Task ExecuteResultAsync(ActionContext context)
        //    {
        //        if (context == null)
        //            throw new ArgumentNullException("context");

        //        var response = context.HttpContext.Response;

        //        response.ContentType = !String.IsNullOrEmpty(ContentType)
        //            ? ContentType
        //            : "application/json";

        //        //if (ContentEncoding != null)
        //        //    response.ContentEncoding = ContentEncoding;

        //        var serializedObject = JsonConvert.SerializeObject(this.Data, Formatting.Indented);
        //        await response.WriteAsync(serializedObject);
        //    }
        //}

        [ValidateAntiForgeryToken]
        [HttpPatch]
        public async Task<IActionResult> ChangeStatus([FromBody]ActiveStatus status)
        {
            await _service.ChangeStatus(status.Id, status.Active);
            return Ok();
        }

        // GET: PartyCategories/Details/5
        public async Task<ActionResult> Details(Guid id)
        {
            if (id == null || id == Guid.Empty)
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
            var party = new PartyCategoryDto { Active = true };
            ViewBag.Mode = "Create";
            ViewBag.Readonly = false;
            return View("Edit", party);
        }

        // GET: PartyCategories/Edit/5
        public async Task<ActionResult> Edit(Guid id)
        {
            if (id == null || id == Guid.Empty)
                return NotFound();

            var party = await _service.GetByIdAsync(id);
            if (party == null)
                return NotFound();

            ViewBag.Mode = "Edit";
            ViewBag.Readonly = false;
            return View(party);
        }

        // POST: PartyCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PartyCategoryDto model)
        {
            try
            {
                var org = HttpContext.Session.Get<OrganizationDto>("DemoOrg");
                if (org == null)
                    return null;

                bool create = model.Id == null || model.Id == Guid.Empty;
                await _service.SaveAsync(org.Id, model);
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

                if (model.Id == null || model.Id == Guid.Empty)
                    ViewBag.Mode = "Create";
                else
                    ViewBag.Mode = "Edit";
                ViewBag.Readonly = false;
                return View("Edit", model);
            }
        }

        [HttpGet]
        // GET: PartyCategories/Delete/5
        public async Task<ActionResult> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty)
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
        public async Task<ActionResult> DeleteConfirmed(Guid id)
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