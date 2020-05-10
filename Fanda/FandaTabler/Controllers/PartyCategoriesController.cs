using Fanda.Dto;
using Fanda.Service;
using Fanda.Service.Base;
using FandaTabler.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Specialized;
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

        public ActionResult Index()
        {
            var org = GetSelectedOrg();
            if (org == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var org = GetSelectedOrg();
                if (org == null)
                {
                    return BadRequest(new { Error = "Org Id is missing" });
                }

                NameValueCollection qFilter = HttpUtility.ParseQueryString(Request.QueryString.Value);
                string search = qFilter["search"];

                var filter = new ChildFilter<IPartyCategoryService, PartyCategoryListDto>(_service, qFilter, search);
                var result = await filter.ApplyAsync(org.Id);
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
                    return BadRequest(new { Error = "Org Id is missing" });
                }

                #region Validation
                if (ModelState.IsValid)
                {
                    var errors = await _service.ValidateAsync(org.Id, model);
                    foreach (var err in errors)
                    {
                        ModelState.AddModelError(err.Key, err.Value);
                    }
                }
                #endregion

                if (ModelState.IsValid)
                {
                    model = await _service.SaveAsync(org.Id, model);
                    return Ok(model);
                }
                else
                {
                    return BadRequest(ModelState);
                }
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