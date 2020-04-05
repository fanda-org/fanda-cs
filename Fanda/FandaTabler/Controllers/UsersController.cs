using Fanda.Dto;
using Fanda.Dto.ViewModels;
using Fanda.Service;
using Fanda.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

using FandaTabler.Extensions;
using System.Linq;

namespace FandaTabler.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        //private readonly AppSettings _appSettings;

        public UsersController(IUserService userService/*, IOptions<AppSettings> options*/)
        {
            _userService = userService;
            //_appSettings = options.Value;
        }

        [AllowAnonymous]
        [HttpGet]
        //[ResponseCache(Duration = 30)]
        public IActionResult Login(string returnUrl = null)
        {
            var model = new LoginViewModel();

            //var persistentClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.IsPersistent);
            //if (persistentClaim != null)
            //{
            //    var isPersistent = persistentClaim.Value;
            //    if (string.IsNullOrEmpty(isPersistent))
            //    {
            //        model.RememberMe = Convert.ToBoolean(isPersistent);
            //    }
            //}
            
            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
            //return Challenge(new AuthenticationProperties() { RedirectUri = "/Home/Index" });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(/*[FromQuery] string returnUrl,*/ [FromForm] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Username or password is missing";
                return View(model);
            }
            var user = await _userService.LoginAsync(model);
            if (user == null)
            {
                TempData["Error"] = "Invalid Username or password";
                return View(model);
            }

            //var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new Claim[]
            //    {
            //        new Claim(ClaimTypes.Name, user.UserId.ToString())
            //    }),
            //    Expires = DateTime.UtcNow.AddDays(7),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //};
            //var token = tokenHandler.CreateToken(tokenDescriptor);
            //var tokenString = tokenHandler.WriteToken(token);
            //user.Token = tokenString;

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.IsPersistent, model.RememberMe.ToString())
            };
            var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(userIdentity);
            var authProps = new AuthenticationProperties
            {
                //AllowRefresh = true,
                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20),
                IsPersistent = model.RememberMe,
                //IssuedUtc = DateTimeOffset.UtcNow,
                //RedirectUri = string.IsNullOrEmpty(returnUrl) ? "/Home/Index" : returnUrl
            };
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                userPrincipal,
                authProps);
            //HttpContext.Session.Set("UserId", user.Id.ToString());

            //await HttpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);

            //await HttpContext.Authentication.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
            //await HttpContext.Authentication.SignInAsync("Cookie", userPrincipal,
            //    new AuthenticationProperties
            //    {
            //        ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
            //        IsPersistent = false,
            //        AllowRefresh = false
            //    });

            //if (Url.IsLocalUrl(returnUrl))
            //    return Redirect(returnUrl);
            //else
            return RedirectToAction("Index", "Home");
            // return basic user info (without password) and token to store client side
            //return Ok(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                //var prop = new AuthenticationProperties
                //{
                //    RedirectUri = "/Users/Login"
                //};
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);  //, prop);
                //HttpContext.Session.Clear();
                //foreach (var cookieKey in Request.Cookies.Keys)
                //{
                //    Response.Cookies.Delete(cookieKey);
                //}
            }
            //return RedirectToAction("Index", "Home");
            return RedirectToAction(nameof(Login));
        }

        [AllowAnonymous]
        //[ResponseCache(Duration = 30)]
        public IActionResult Register(string userId, string code, string page)
        {
            if (!string.IsNullOrEmpty(userId) &&
               !string.IsNullOrEmpty(code) &&
               !string.IsNullOrEmpty(page))
            {
                return RedirectToAction(page);
            }
            else
            {
                return View(new RegisterViewModel());
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromForm]RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string token = Guid.NewGuid().ToString();
            string callbackUrl = Url.Page(
                "/Users/ConfirmEmail",
                pageHandler: null,
                values: new { userId = model.UserName, code = token },
                protocol: Request.Scheme
            );

            // save
            _ = await _userService.RegisterAsync(model, callbackUrl);
            return RedirectToAction(nameof(Login));
        }

        [AllowAnonymous]
        //[ResponseCache(Duration = 30)]
        public IActionResult ConfirmEmail() => View();

        [AllowAnonymous]
        //[ResponseCache(Duration = 30)]
        public IActionResult ForgotPassword() => View();

        //[HttpGet]
        public async Task<IActionResult> GetAll(Guid orgId/*, bool? active*/)
        {
            var users = await _userService.GetAllAsync(orgId/*, active*/);
            return Ok(users);
        }

        //[HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            return Ok(user);
        }

        //[HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody]UserDto model, string password)
        {
            try
            {
                // save 
                await _userService.SaveAsync(model, password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        //[HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid orgId, Guid userId)
        {
            await _userService.DeleteAsync(orgId, userId);
            return Ok();
        }

        [AllowAnonymous]
        //[ResponseCache(Duration = 300)]
        public IActionResult Terms() => View();
    }
}