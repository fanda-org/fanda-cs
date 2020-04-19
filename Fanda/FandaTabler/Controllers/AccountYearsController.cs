using Fanda.Dto;
using Fanda.Service;
using Fanda.Shared;
using FandaTabler.Extensions;
using FandaTabler.Models;
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
        public async Task<ActionResult> GetAll(Guid orgId)
        {
            try
            {
                if (orgId == Guid.Empty)
                {
                    var org = GetSelectedOrg();
                    if (org == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    orgId = org.Id;
                }

                NameValueCollection qFilter = HttpUtility.ParseQueryString(Request.QueryString.Value);
                string search = qFilter["search"];
                var filter = new BaseOrgFilter<IAccountYearService, YearListDto>
                {
                    PageIndex = string.IsNullOrEmpty(qFilter["pageIndex"]) ? 1 : Convert.ToInt32(qFilter["pageIndex"]),
                    PageSize = string.IsNullOrEmpty(qFilter["pageSize"]) ? 100 : Convert.ToInt32(qFilter["pageSize"]),
                    SortField = qFilter["sortField"],
                    SortOrder = qFilter["sortOrder"],
                    Code = string.IsNullOrEmpty(qFilter["code"]) ? search : qFilter["code"],
                    Name = string.IsNullOrEmpty(qFilter["name"]) ? search : qFilter["name"],
                    Description = string.IsNullOrEmpty(qFilter["description"]) ? search : qFilter["description"],
                    Active = string.IsNullOrEmpty(qFilter["active"]) ? (bool?)null : bool.Parse(qFilter["Active"])
                };

                PagedList<YearListDto> data;
                if (string.IsNullOrEmpty(search))
                {
                    data = await filter.ApplyAllAsync(_service, orgId);
                }
                else
                {
                    data = await filter.ApplyAnyAsync(_service, orgId);
                }

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
