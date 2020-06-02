using Fanda.Dto;
using Fanda.Repository;
using Fanda.Repository.Base;
using Fanda.Repository.Extensions;
using FandaTabler.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web;

namespace FandaTabler.Controllers
{
    [Authorize]
    public class ProductBrandsController : Controller
    {
        private readonly IProductBrandRepository _repository;

        public ProductBrandsController(IProductBrandRepository repository)
        {
            _repository = repository;
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
                //string search = qFilter["search"];
                //var filter = new ChildFilter<IProductBrandRepository, ProductBrandListDto>(_repository, qFilter, search);
                //var result = await filter.ApplyAsync(org.Id);
                var response = await _repository
                    .GetPaged(org.Id, new Query
                    {
                        Page = Convert.ToInt32(qFilter["pageIndex"]),
                        PageSize = Convert.ToInt32(qFilter["pageSize"]),
                        Search = qFilter["search"]
                    });
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(ProductBrandDto model)
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
                    var errors = await _repository.ValidateAsync(org.Id, model);
                    //foreach (var err in errors)
                    //{
                    //    ModelState.AddModelError(err.Key, err.Value);
                    //}
                    foreach (var field in errors)
                    {
                        foreach (var value in field.Value)
                        {
                            ModelState.AddModelError(field.Key, value);
                        }
                    }
                }
                #endregion

                if (ModelState.IsValid)
                {
                    if (model.Id == null || model.Id == Guid.Empty)
                    {
                        model = await _repository.CreateAsync(org.Id, model);
                    }
                    else
                    {
                        await _repository.UpdateAsync(model.Id, model);
                    }
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
                await _repository.DeleteAsync(new Guid(id));
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