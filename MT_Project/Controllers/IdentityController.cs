using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using MT_app.business.Services;
using MT_app.Infrastructure.Data.AuthenModels;
using System.Text;

namespace MT_Project.Controllers
{
    public class IdentityController : Controller
    {
        private IIdentityService _identityService;
        private readonly ILogger<AppUserIdentityViewModel> _logger;

        public IdentityController(
            IIdentityService identityService,
            ILogger<AppUserIdentityViewModel> logger)
        {
            this._identityService = identityService;
            _logger = logger;
        }

        public IActionResult LoginRegister(string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            ViewData["formSelected"] = "Login";

            return View();
        }

        public IActionResult AccessDenied(string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            ViewData["formSelected"] = "Login";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AppUserIdentityViewModel viewModel)
        {

            var result = await _identityService.Login(viewModel);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                // HttpContext.Session.SetString("userLogin", "Value");
                return Redirect(viewModel.ReturnUrl);
            }

            if (_identityService.IsConfirmEmailRequired(viewModel).Result)
            {
                ModelState.AddModelError(string.Empty, "Please check your email to confirm your account");

                ViewData["formSelected"] = "Login";
                ViewData["returnUrl"] = viewModel.ReturnUrl;
                return View("LoginRegister", viewModel);
            }

            if (result.RequiresTwoFactor)
            {
                return RedirectToPage("./LoginWith2fa",
                    new { ReturnUrl = viewModel.ReturnUrl, RememberMe = viewModel.LoginInput.RememberMe });
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return RedirectToPage("./Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            ViewData["formSelected"] = "Login";
            ViewData["returnUrl"] = viewModel.ReturnUrl;
            return View("LoginRegister", viewModel);

        }

        [HttpPost]
        public async Task<IActionResult> Register(AppUserIdentityViewModel model)
        {
            var result = await _identityService.Register(model);

            if (result.Errors.Any())
            {
                ModelState.Clear();
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                ViewData["formSelected"] = "Register";
                ViewData["returnUrl"] = model.ReturnUrl;
                return View("LoginRegister", model);
            }
            ViewData["formSelected"] = "Login";
            ViewData["returnUrl"] = model.ReturnUrl;
            ViewData["Messenger"] = "Please check your email to confirm your account!";
            return View("LoginRegister", model);

        }

        public async Task<IActionResult> ConfirmEmail(string userId, string code, string returnUrl)
        {
            IdentityResult result = await _identityService.ConfirmEmail(userId, code);
            string StatusMessage = result.Succeeded
                ? "Thank you for confirming your email. Please login!"
                : "Error confirming your email.";
            ViewData["StatusMessage"] = StatusMessage;
            return RedirectToAction("LoginRegister", new { returnUrl = returnUrl });

        }


        public IActionResult Logout()
        {
            return null;
        }
    }
}