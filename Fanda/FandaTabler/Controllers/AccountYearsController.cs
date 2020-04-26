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
    public class AccountYearsController : Controller
    {
        private readonly IAccountYearService _service;

        public AccountYearsController(IAccountYearService service)
        {
            _service = service;
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
                string search = qFilter["search"];
                
                var filter = new OrgFilter<IAccountYearService, YearListDto>(_service, qFilter, search);
                var data = await filter.ApplyAsync(id);
                var result = new JsGridResult<IList<YearListDto>> { Data = data.List, ItemsCount = data.RowCount };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
            }
        }

        private OrganizationDto GetSelectedOrg() => HttpContext.Session.Get<OrganizationDto>("CurrentOrg");
    }
}
