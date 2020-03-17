using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Fanda.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Fanda.Service.Business;
using System.Linq;
using Fanda.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Fanda.ViewModel.Business;

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
