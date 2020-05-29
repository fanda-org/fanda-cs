using Fanda.Dto;
using Fanda.Repository;
using Fanda.Repository.Base;
using Fanda.Repository.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Fanda.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UnitsController : ControllerBase
    {
        private readonly IUnitRepository repository;
        public UnitsController(IUnitRepository repository)
        {
            this.repository = repository;
        }

        // GET: api/<UnitsController>
        [HttpGet]
        public async Task<IEnumerable<UnitListDto>> Get()
        {
            NameValueCollection queryString = HttpUtility.ParseQueryString(Request.QueryString.Value);
            var response = await repository
                .GetList(new Guid("3F60039B-8EAF-49B2-4D14-08D7CED444AC"),
                    new Query
                    {
                        Filter = queryString["filter"],
                        FilterArgs = queryString["filterArgs"]?.Split(','),
                        Page = Convert.ToInt32(queryString["page"]),
                        PageSize = Convert.ToInt32(queryString["pageSize"]),
                        Search = queryString["search"],
                        Sort = queryString["sort"],
                    });
            return response.Data;
            //NameValueCollection qFilter = HttpUtility.ParseQueryString(Request.QueryString.Value);
            //if (!string.IsNullOrEmpty(qFilter["filterBy"]))
            //{
            //    qry = qry.Where("Code==@0", "KGM");
            //}
            //return qry.ToListAsync();
        }

        // GET api/<UnitsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UnitsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UnitsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UnitsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
