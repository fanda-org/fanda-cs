using AutoMapper;
using Fanda.Data.Access;
using Fanda.Service.Utility;
using Fanda.ViewModel.Access;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Fanda.Service.Access
{
    public interface IAccountService
    {
        Task<ViewModel.Access.IdentityResult> RegisterAsync(RegisterViewModel model);

        Task<ViewModel.Access.SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();
    }

    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;

        public AccountService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailSender emailSender,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _mapper = mapper;
        }

        public string ErrorMessage { get; set; }

        // POST api/accounts
        public async Task<ViewModel.Access.IdentityResult> RegisterAsync(RegisterViewModel model)
        {
            List<ViewModel.Access.IdentityError> errors = new List<ViewModel.Access.IdentityError>();
            if (string.IsNullOrEmpty(model.Email))
                errors.Add(new ViewModel.Access.IdentityError() { Code = "Email", Description = "Email is required" });
            if (!IsEmail(model.Email))
                errors.Add(new ViewModel.Access.IdentityError() { Code = "Email", Description = "Email is not valid format" });
            if (string.IsNullOrEmpty(model.UserName))
                errors.Add(new ViewModel.Access.IdentityError() { Code = "UserName", Description = "User name is required" });
            if (string.IsNullOrEmpty(model.Password))
                errors.Add(new ViewModel.Access.IdentityError() { Code = "Password", Description = "Password is required" });
            if (model.Password != model.ConfirmPassword)
                errors.Add(new ViewModel.Access.IdentityError() { Code = "ConfirmPassword", Description = "The password and confirm password do not match" });

            if (errors.Any())
                return ViewModel.Access.IdentityResult.Failed(errors.ToArray());

            var userIdentity = _mapper.Map<User>(model);

            Microsoft.AspNetCore.Identity.IdentityResult result = await _userManager.CreateAsync(userIdentity, model.Password);
            if (result.Succeeded)
            {
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(userIdentity);
                var callbackUrl = $"~/Account/ConfirmEmail/{userIdentity.Id}/{code}"; //Url.Action("ConfirmEmail", "Account", new { userId = userIdentity.Id, code }, protocol: HttpContext.Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                    "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");

                await _signInManager.SignInAsync(userIdentity, isPersistent: false);
                //return GenerateJwtToken(userIdentity.Email, userIdentity);
            }
            return new ViewModel.Access.IdentityResult { Succeeded = result.Succeeded, Errors = null };
            //if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));
            //await _appDbContext.Customers.AddAsync(new Customer { IdentityId = userIdentity.Id, Location = model.Location });
            //await _appDbContext.SaveChangesAsync();
            //return new OkObjectResult("Account created");
        }

        public async Task<ViewModel.Access.SignInResult> LoginAsync(LoginViewModel model)
        {
            try
            {
                // Clear the existing external cookie to ensure a clean login process
                //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
                await LogoutAsync();

                User user = null;

                if (IsEmail(model.NameOrEmail))
                    user = await _userManager.FindByEmailAsync(model.NameOrEmail);
                else
                    user = await _userManager.FindByNameAsync(model.NameOrEmail);
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
                user.DateLastLogin = DateTime.Now;
                await _userManager.UpdateAsync(user);
                //if (result.Succeeded)
                //{
                //    return await GenerateJwtToken(model.NameOrEmail, user);
                //}
                return new ViewModel.Access.SignInResult
                {
                    IsLockedOut = result.IsLockedOut,
                    IsNotAllowed = result.IsNotAllowed,
                    RequiresTwoFactor = result.RequiresTwoFactor,
                    Succeeded = result.Succeeded
                };
                //if (result.Succeeded)
                //{
                //    //_logger.LogInformation("User logged in.");
                //    //return RedirectToLocal(returnUrl);
                //    return result;
                //}
                ////if (result.RequiresTwoFactor)
                ////{
                ////    return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, model.RememberMe });
                ////}
                //if (result.IsLockedOut)
                //{
                //    //_logger.LogWarning("User account locked out.");
                //    //return RedirectToAction(nameof(Lockout));
                //    return StatusCode(500, result);
                //}
                //else
                //{
                //    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                //    return NotFound(new { ErrorMessage = "Invalid login attempt" });
                //}
            }
            catch (Exception ex)
            {
                // If we got this far, something failed, redisplay form
                //return BadRequest(new { ErrorMessage = ex.Message });
                throw ex;
            }
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        //private async Task<object> GenerateJwtToken(string email, User user)
        //{
        //    var claims = new List<Claim>
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, email),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        //    };

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //    var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

        //    var token = new JwtSecurityToken(
        //        _configuration["JwtIssuer"],
        //        _configuration["JwtIssuer"],
        //        claims,
        //        expires: expires,
        //        signingCredentials: creds
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}

        private bool IsEmail(string emailString)
        {
            return Regex.IsMatch(emailString,
                @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                RegexOptions.IgnoreCase);
        }
    }
}