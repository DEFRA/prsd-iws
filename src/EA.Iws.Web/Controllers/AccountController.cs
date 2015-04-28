namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Microsoft.Owin.Security;
    using Thinktecture.IdentityModel.Client;
    using ViewModels.Account;
    using TokenResponse = Api.Client.Entities.TokenResponse;

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

            var client = oauthClient();
            var response = await client.GetAccessTokenAsync(model.Email, model.Password);
            var identity = GenerateUserIdentity(response);
            var authProperties = new AuthenticationProperties { IsPersistent = model.RememberMe };

            authenticationManager.SignIn(authProperties, identity);

            return RedirectToLocal(returnUrl);
        }

        private static ClaimsIdentity GenerateUserIdentity(TokenResponse response)
        {
            var identity = new ClaimsIdentity(Constants.IwsAuthType);
            identity.AddClaim(new Claim(OAuth2Constants.AccessToken, response.AccessToken));
            if (response.IdentityToken != null)
            {
                identity.AddClaim(new Claim(OAuth2Constants.IdentityToken, response.IdentityToken));
            }
            identity.AddClaim(new Claim(IwsClaimTypes.ExpiresAt,
                DateTimeOffset.Now.AddSeconds(response.ExpiresIn).ToString()));
            return identity;
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