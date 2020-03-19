﻿using Fanda.Service;
using Fanda.Shared;
using FandaCoreUI.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace FandaCoreUI.Controllers
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

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}