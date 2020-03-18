using Fanda.Common.Extensions;
using Fanda.Common.Models;
using Fanda.Service.Business;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace Fanda.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOrganizationService _organizationService;
        public HomeController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }
        public IActionResult Index()
        {
            var demoOrg = _organizationService
                .GetAll()
                .Where(o => o.OrgCode == "DEMO")
                .FirstOrDefault();

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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}