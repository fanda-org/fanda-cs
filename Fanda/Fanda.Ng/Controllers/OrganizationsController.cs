using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web;
using Fanda.Base;
using Fanda.Repository;
using Fanda.Repository.Base;
using Fanda.Repository.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fanda.Controllers
{
    public class OrganizationsController : BaseController
    {
        private readonly IOrganizationRepository repository;

        public OrganizationsController(IOrganizationRepository repository)
        {
            this.repository = repository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            NameValueCollection queryString = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var response = await repository
                .GetList(new Guid("08d80915-9078-44e3-8d9f-7802fa63c1d1"),
                    new Query
                    {
                        Filter = queryString["filter"],
                        FilterArgs = queryString["filterArgs"]?.Split(','),
                        Page = Convert.ToInt32(queryString["page"]),
                        PageSize = Convert.ToInt32(queryString["pageSize"]),
                        Search = queryString["search"],
                        Sort = queryString["sort"],
                    });
            return Ok(response);
        }
    }
}