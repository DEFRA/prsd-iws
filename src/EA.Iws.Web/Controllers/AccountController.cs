namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Api.Client.Entities;
    using Infrastructure;
    using Microsoft.Owin.Security;
    using Prsd.Core.Web.OAuth;
    using Services;
    using ViewModels.Account;

    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAuthenticationManager authenticationManager;
        private readonly Func<IOAuthClient> oauthClient;
        private readonly Func<IIwsClient> apiClient;
        private readonly IEmailService emailService;

        public AccountController(Func<IOAuthClient> oauthClient,
            IAuthenticationManager authenticationManager,
            Func<IIwsClient> apiClient,
            IEmailService emailService)
        {
            this.oauthClient = oauthClient;
            this.apiClient = apiClient;
            this.authenticationManager = authenticationManager;
            this.emailService = emailService;
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
                authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = model.RememberMe },
                    response.GenerateUserIdentity());
                return RedirectToLocal(returnUrl);
            }
            ModelState.AddModelError(string.Empty, "The username or password is incorrect");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            authenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Home", "Applicant");
        }

        [HttpGet]
        public ActionResult EmailVerificationRequired()
        {
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

            return RedirectToAction("EmailVerificationRequired");
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