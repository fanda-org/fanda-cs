using Fanda.Dto;
using Fanda.Dto.Base;
using Fanda.Service;
using Fanda.Service.Base;
using FandaCoreUI.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FandaCoreUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOrganizationService _organizationService;
        public HomeController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }
        public async Task<IActionResult> Index()
        {
            var demoOrg = await ((IListService<OrgListDto>)_organizationService)
                .GetAll()
                .Where(o => o.Code == "DEMO")
                .FirstOrDefaultAsync();

            HttpContext.Session.Set("DemoOrg", demoOrg);
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorDto { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}