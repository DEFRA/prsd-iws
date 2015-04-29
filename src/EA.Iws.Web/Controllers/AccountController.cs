namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Microsoft.Owin.Security;
    using ViewModels.Account;

    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAuthenticationManager authenticationManager;
        private readonly Func<IwsOAuthClient> oauthClient;

        public AccountController(Func<IwsOAuthClient> oauthClient, IAuthenticationManager authenticationManager)
        {
            this.oauthClient = oauthClient;
            this.authenticationManager = authenticationManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await oauthClient().GetAccessTokenAsync(model.Email, model.Password);
            if (response.AccessToken != null)
            {
                authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = model.RememberMe }, response.GenerateUserIdentity());
                return RedirectToLocal(returnUrl);
            }
            ModelState.AddModelError(string.Empty, "The username or password is incorrect");
            return View(model);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}