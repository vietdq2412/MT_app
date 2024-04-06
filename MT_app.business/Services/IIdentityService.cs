using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using MT_app.Infrastructure.Data.AuthenModels;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.Http;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using MT_app.core.Models;

namespace MT_app.business.Services
{
    public interface IIdentityService
    {
        Task<IdentityResult> Register(AppUserIdentityViewModel model);
        Task<SignInResult> Login(AppUserIdentityViewModel model);

        Task<IdentityResult> ConfirmEmail(string userId, string code);

        Task<bool> IsConfirmEmailRequired(AppUserIdentityViewModel viewModel);
        void Logout();
        void ResetPassword();
    }

    public class IdentityService : IIdentityService
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IUserStore<AppUser> _userStore;
        private readonly IUserEmailStore<AppUser> _emailStore;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AppUserIdentityViewModel> _logger;

        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IdentityService(
            SignInManager<AppUser> signInManager,
            ILogger<AppUserIdentityViewModel> logger,
            UserManager<AppUser> userManager,
            IUserStore<AppUser> userStore,
            IEmailSender emailSender,
            IUrlHelperFactory urlHelperFactory,
            IHttpContextAccessor httpContextAccessor,
            RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _emailSender = emailSender;
            _logger = logger;
            _urlHelperFactory = urlHelperFactory;
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> Register(AppUserIdentityViewModel model)
        {
            var user = CreateUser();
            await _userStore.SetUserNameAsync(user, model.RegisterInput.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, model.RegisterInput.Email, CancellationToken.None);
            user.FirstName = model.RegisterInput.FirstName;
            user.LastName = model.RegisterInput.LastName;
            if (await _roleManager.RoleExistsAsync("Staff"))
            {
                var result = await _userManager.CreateAsync(user, model.RegisterInput.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    SendConfirmEmail(user, model);
                }
                await _userManager.AddToRoleAsync(user, "Staff");

                return result;
            }

            return IdentityResult.Failed();
        }

        private async void SendConfirmEmail(AppUser user, AppUserIdentityViewModel model)
        {
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var httpContext = _httpContextAccessor.HttpContext;
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
            var urlHelper = _urlHelperFactory.GetUrlHelper(actionContext);

            var callbackUrl = urlHelper.Action(
                "ConfirmEmail",
                "Identity",
                new { userId = userId, code = code, returnUrl = model.ReturnUrl },
                protocol: "https"
            );

            await _emailSender.SendEmailAsync(model.RegisterInput.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
        }

        public async Task<IdentityResult> ConfirmEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed();
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return result;
        }

        public async Task<bool> IsConfirmEmailRequired(AppUserIdentityViewModel viewModel)
        {
            var user = await _userManager.FindByEmailAsync(viewModel.LoginInput.Email);

            // Check if the user exists and email is confirmed
            return user is { EmailConfirmed: false };
        }


        public async Task<SignInResult> Login(AppUserIdentityViewModel model)
        {
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager
                .PasswordSignInAsync(model.LoginInput.Email, model.LoginInput.Password, model.LoginInput.RememberMe,
                    lockoutOnFailure: false);
            return result;
        }


        public void Logout()
        {
            throw new NotImplementedException();
        }

        public void ResetPassword()
        {
            throw new NotImplementedException();
        }


        private IUserEmailStore<AppUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }

            return (IUserEmailStore<AppUser>)_userStore;
        }

        private AppUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<AppUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(AppUser)}'. " +
                                                    $"Ensure that '{nameof(AppUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                                                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
    }
}