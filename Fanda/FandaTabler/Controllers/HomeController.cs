using Fanda.Common.Extensions;
using Fanda.Common.Models;
using Fanda.ViewModel.Business;
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
            if (HttpContext.Session.Get<OrganizationViewModel>("CurrentOrg") == null)
                return RedirectToAction("Index", "Organizations");
            else
                return View();
        }

        [AllowAnonymous]
        //[ResponseCache(CacheProfileName = "Default")]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        //[ResponseCache(CacheProfileName = "Default")]
        public IActionResult Docs()
        {
            return View();
        }

        [AllowAnonymous]
        //[ResponseCache(CacheProfileName = "Default")]
        public IActionResult FAQ()
        {
            return View();
        }

        [AllowAnonymous]
        //[ResponseCache(CacheProfileName = "NoCache")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
