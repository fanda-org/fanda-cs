﻿using Fanda.Dto;
using Fanda.Shared;
using FandaTabler.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

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
            //if (string.IsNullOrEmpty(HttpContext.Session.Get<string>("UserId")))
            //{
            //    return RedirectToAction("Login", "Users");
            //}
            //else
            //{
            if (User.Identity.IsAuthenticated)
            {
                if (HttpContext.Session.Get<OrganizationDto>("CurrentOrg") == null)
                {
                    return RedirectToAction("Index", "Organizations");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login", "Users");
            }
            //}
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
