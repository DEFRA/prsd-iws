namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Net.Mail;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Api.Client.Entities;
    using Infrastructure;
    using Microsoft.Owin.Security;
    using Prsd.Core.Web.OAuth;
    using Prsd.Core.Web.OpenId;
    using Services;
    using ViewModels.Account;

    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAuthenticationManager authenticationManager;
        private readonly Func<IOAuthClient> oauthClient;
        private readonly Func<IIwsClient> apiClient;
        private readonly IEmailService emailService;
        private readonly Func<IUserInfoClient> userInfoClient;

        public AccountController(Func<IOAuthClient> oauthClient,
            IAuthenticationManager authenticationManager,
            Func<IIwsClient> apiClient,
            IEmailService emailService,
            Func<IUserInfoClient> userInfoClient)
        {
            this.oauthClient = oauthClient;
            this.apiClient = apiClient;
            this.authenticationManager = authenticationManager;
            this.emailService = emailService;
            this.userInfoClient = userInfoClient;
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
                var identity = response.GenerateUserIdentity();

                authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = model.RememberMe },
                    identity);

                bool isInternal = await IsInternalUser(response.AccessToken);

                return RedirectToLocal(returnUrl, isInternal);
            }
            ModelState.AddModelError(string.Empty, "The username or password is incorrect");
            return View(model);
        }

        private async Task<bool> IsInternalUser(string accessToken)
        {
            var userInfo = await userInfoClient().GetUserInfoAsync(accessToken);

            return userInfo.Claims.Any(p => p.Item1 == ClaimTypes.Role && p.Item2 == "internal");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            authenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private ActionResult RedirectToLocal(string returnUrl, bool isInternal)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return isInternal ? RedirectToAction("Index", "Home", new { area = "Admin" }) : RedirectToAction("Home", "Applicant");
        }

        [HttpGet]
        public ActionResult EmailVerificationRequired()
        {
            var identity = (ClaimsIdentity)User.Identity;

            ViewBag.Email = identity.Claims.Single(c => c.Type.Equals(ClaimTypes.Email)).Value;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EmailVerificationRequired(FormCollection model)
        {
            try
            {
                using (var client = apiClient())
                {
                    var verificationToken = await client.Registration.GetUserEmailVerificationTokenAsync(User.GetAccessToken());
                    var verificationEmail =
                        emailService.GenerateEmailVerificationMessage(
                            Url.Action("VerifyEmail", "Account", null, Request.Url.Scheme),
                            verificationToken, User.GetUserId(), User.GetEmailAddress());
                    var emailSent = await emailService.SendAsync(verificationEmail);

                    if (!emailSent)
                    {
                        ViewBag.Errors = new[] { "Email is currently unavailable at this time, please try again later." };
                        return View();
                    }
                }
            }
            catch (SmtpException)
            {
                ViewBag.Errors = new[] { "The verification email was not sent, please try again later." };
                return View();
            }

            return RedirectToAction("EmailVerificationResent");
        }

        [HttpGet]
        public ActionResult EmailVerificationResent()
        {
            var identity = (ClaimsIdentity)User.Identity;

            ViewBag.Email = identity.Claims.Single(c => c.Type.Equals(ClaimTypes.Email)).Value;

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> VerifyEmail(Guid id, string code)
        {
            using (var client = apiClient())
            {
                bool result = await client.Registration.VerifyEmailAsync(new VerifiedEmailData { Id = id, Code = code });

                if (!result)
                {
                    return RedirectToAction("EmailVerificationRequired");
                }
            }

            return View();
        }
    }
}