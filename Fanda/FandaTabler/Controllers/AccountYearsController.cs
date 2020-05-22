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
using System.Threading.Tasks;
using System.Web;

namespace FandaTabler.Controllers
{
    [Authorize]
    public class AccountYearsController : Controller
    {
        private readonly IAccountYearRepository _repository;

        public AccountYearsController(IAccountYearRepository service)
        {
            _repository = service;
        }

        public IActionResult Index() => View();

        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult> GetAll(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    var org = GetSelectedOrg();
                    if (org == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    id = org.Id;
                }

                NameValueCollection qFilter = HttpUtility.ParseQueryString(Request.QueryString.Value);
                //string search = qFilter["search"];
                //var filter = new ChildFilter<IAccountYearRepository, YearListDto>(_service, qFilter, search);
                //var result = await filter.ApplyAsync(id);
                var response = await _repository
                   .GetList(id, new Query { });

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
            }
        }

        private OrganizationDto GetSelectedOrg() => HttpContext.Session.Get<OrganizationDto>("CurrentOrg");
    }
}
