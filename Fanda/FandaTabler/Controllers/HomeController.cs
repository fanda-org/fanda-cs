using Fanda.Dto;
using Fanda.Shared.Models;
using FandaTabler.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FandaTabler.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        [Authorize]
        //[ResponseCache(CacheProfileName = "Default")]
        public IActionResult Index()
        {
            if (HttpContext.Session.Get<OrganizationDto>("CurrentOrg") == null)
                return RedirectToAction("Index", "Organizations");
            else
                return View();
        }

        [AllowAnonymous]
        //[ResponseCache(CacheProfileName = "Default")]
        public IActionResult Privacy() => View();

        [AllowAnonymous]
        //[ResponseCache(CacheProfileName = "Default")]
        public IActionResult Docs() => View();

        [AllowAnonymous]
        //[ResponseCache(CacheProfileName = "Default")]
        public IActionResult FAQ() => View();

        [AllowAnonymous]
        //[ResponseCache(CacheProfileName = "NoCache")]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
