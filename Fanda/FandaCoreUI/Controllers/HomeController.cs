using Fanda.Dto;
using Fanda.Dto.Base;
using Fanda.Repository;
using Fanda.Repository.Base;
using FandaCoreUI.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FandaCoreUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOrganizationRepository _repository;
        private readonly IUserRepository _userRepository;
        public HomeController(IOrganizationRepository repository, IUserRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }
        public async Task<IActionResult> Index()
        {
            var fandaAdmin = await _userRepository.GetAll()
                .FirstOrDefaultAsync(u => u.Name == "FandaAdmin");

            var demoOrg = await ((IRepositoryChildList<OrgListDto>)_repository)
                .GetAll(fandaAdmin.Id)
                .FirstOrDefaultAsync(o => o.Code == "DEMO");

            HttpContext.Session.Set("DemoOrg", demoOrg);
            return View();
        }
        private Guid GetCurrentUserId()
        {
            string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            Guid userGuid = new Guid(userId);
            if (userGuid == null || userGuid == Guid.Empty)
                throw new ApplicationException("UserId not found in logged in user identity");

            return userGuid;
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