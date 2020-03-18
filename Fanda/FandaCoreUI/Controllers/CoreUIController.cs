using Microsoft.AspNetCore.Mvc;

namespace Fanda.Mvc.Controllers
{
    [Route("CoreUI")]
    public class CoreUIController : Controller
    {
        [Route("{view=Index}")]
        public IActionResult Index(string view)
        {
            ViewData["Title"] = "Fanda - Finance and Accounting System";

            return View(view);
        }
    }
}