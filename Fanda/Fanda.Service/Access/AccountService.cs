using AutoMapper;
using Fanda.Common.Helpers;
using Fanda.Data.Access;
using Fanda.Service.Utility;
using Fanda.ViewModel.Access;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Fanda.Service.Access
{
    public interface IAccountService
    {
        //Task<ViewModel.Access.IdentityResult> RegisterAsync(RegisterViewModel model);
        Task<UserViewModel> LoginAsync(LoginViewModel model);

        //Task LogoutAsync();
    }

    public class AccountService : IAccountService
    {
        private readonly AppSettings _appSettings;
        private readonly UserService _userService;

        //private readonly UserManager<User> _userManager;
        //private readonly SignInManager<User> _signInManager;
        //private readonly IEmailSender _emailSender;
        //private readonly IMapper _mapper;

        public AccountService(
            IOptions<AppSettings> appSettings,
            UserService userService,
            //UserManager<User> userManager,
            //SignInManager<User> signInManager,
            IEmailSender emailSender,
            IMapper mapper)
        {
            _appSettings = appSettings.Value;
            _userService = userService;

            //_userManager = userManager;
            //_signInManager = signInManager;
            //_emailSender = emailSender;
            //_mapper = mapper;
        }

        public string ErrorMessage { get; set; }

        // POST api/accounts
        //public async Task<ViewModel.Access.IdentityResult> RegisterAsync(RegisterViewModel model)
        //{
        //    List<ViewModel.Access.IdentityError> errors = new List<ViewModel.Access.IdentityError>();
        //    if (string.IsNullOrEmpty(model.Email))
        //        errors.Add(new ViewModel.Access.IdentityError() { Code = "Email", Description = "Email is required" });
        //    if (!IsEmail(model.Email))
        //        errors.Add(new ViewModel.Access.IdentityError() { Code = "Email", Description = "Email is not valid format" });
        //    if (string.IsNullOrEmpty(model.UserName))
        //        errors.Add(new ViewModel.Access.IdentityError() { Code = "UserName", Description = "User name is required" });
        //    if (string.IsNullOrEmpty(model.Password))
        //        errors.Add(new ViewModel.Access.IdentityError() { Code = "Password", Description = "Password is required" });
        //    if (model.Password != model.ConfirmPassword)
        //        errors.Add(new ViewModel.Access.IdentityError() { Code = "ConfirmPassword", Description = "The password and confirm password do not match" });

        //    if (errors.Any())
        //        return ViewModel.Access.IdentityResult.Failed(errors.ToArray());

        //    var userIdentity = _mapper.Map<User>(model);

        //    Microsoft.AspNetCore.Identity.IdentityResult result = await _userManager.CreateAsync(userIdentity, model.Password);
        //    if (result.Succeeded)
        //    {
        //        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
        //        // Send an email with this link
        //        var code = await _userManager.GenerateEmailConfirmationTokenAsync(userIdentity);
        //        var callbackUrl = $"~/Account/ConfirmEmail/{userIdentity.Id}/{code}"; //Url.Action("ConfirmEmail", "Account", new { userId = userIdentity.Id, code }, protocol: HttpContext.Request.Scheme);
        //        await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
        //            "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");

        //        //await _signInManager.SignInAsync(userIdentity, isPersistent: false);
        //        //return GenerateJwtToken(userIdentity.Email, userIdentity);
        //    }
        //    return new ViewModel.Access.IdentityResult { Succeeded = result.Succeeded, Errors = null };
        //    //if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));
        //    //await _appDbContext.Customers.AddAsync(new Customer { IdentityId = userIdentity.Id, Location = model.Location });
        //    //await _appDbContext.SaveChangesAsync();
        //    //return new OkObjectResult("Account created");
        //}

        public async Task<UserViewModel> LoginAsync(LoginViewModel model)
        {
            User user = await _userService.GetByPasswordAsync(model.NameOrEmail, model.Password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Password = null;

            return user;
        }

        //public async Task LogoutAsync()
        //{
        //    await _signInManager.SignOutAsync();
        //}

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

    }
}