using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FandaTabler.Controllers
{
    [AllowAnonymous]
    public class ErrorsController : Controller
    {
        public IActionResult Index([FromRoute] int id)
        {
            string message = ((HttpStatusCode)id).ToString();
            TempData["Code"] = id;
            TempData["Message"] = message;
            return View();
        }
    }
}