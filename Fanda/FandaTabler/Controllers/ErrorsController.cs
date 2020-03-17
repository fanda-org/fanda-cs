using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FandaTabler.Controllers
{
    [AllowAnonymous]
    public class ErrorsController : Controller
    {
        public IActionResult Index([FromRoute]int id)
        {
            string message = ((HttpStatusCode)id).ToString();
            TempData["Code"] = id;
            TempData["Message"] = message;
            return View();
        }
    }
}