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
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Prsd.Core.Web.OAuth;
    using Prsd.Core.Web.OpenId;
    using ViewModels.Account;

    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAuthenticationManager authenticationManager;
        private readonly IOAuthClient oauthClient;
        private readonly IIwsClient client;
        private readonly IUserInfoClient userInfoClient;

        public AccountController(IOAuthClient oauthClient,
            IAuthenticationManager authenticationManager,
            IIwsClient client,
            IUserInfoClient userInfoClient)
        {
            this.oauthClient = oauthClient;
            this.client = client;
            this.authenticationManager = authenticationManager;
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

            var response = await oauthClient.GetAccessTokenAsync(model.Email, model.Password);
            if (response.AccessToken != null)
            {
                var identity = response.GenerateUserIdentity();

                authenticationManager.SignIn(new AuthenticationProperties(), identity);

                bool isInternal = await IsInternalUser(response.AccessToken);

                return RedirectToLocal(returnUrl, isInternal);
            }
            ModelState.AddModelError(string.Empty, "The username or password is incorrect");
            return View(model);
        }

        private async Task<bool> IsInternalUser(string accessToken)
        {
            var userInfo = await userInfoClient.GetUserInfoAsync(accessToken);

            return userInfo.Claims.Any(p => p.Item1 == ClaimTypes.Role && p.Item2 == "internal");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            authenticationManager.SignOut(Constants.IwsAuthType);
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
                var emailSent =
                await
                    client.Registration.SendEmailVerificationAsync(User.GetAccessToken(),
                        new EmailVerificationData
                        {
                            Url = Url.Action("VerifyEmail", "Account", null, Request.Url.Scheme)
                        });

                if (!emailSent)
                {
                    ViewBag.Errors = new[] { "Email is currently unavailable at this time, please try again later." };
                    return View();
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
            bool result = await client.Registration.VerifyEmailAsync(new VerifiedEmailData { Id = id, Code = code });

            if (!result)
            {
                return RedirectToAction("EmailVerificationRequired");
            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await client.Registration.ResetPasswordRequestAsync(
                new PasswordResetRequest
                {
                    EmailAddress = model.Email,
                    Url = Url.Action("ResetPassword", "Account", null, Request.Url.Scheme)
                });

            if (!result)
            {
                ModelState.AddModelError("Email", "Email address not recognised.");
                return View(model);
            }

            return RedirectToAction("ResetPasswordEmailSent", "Account", new { email = model.Email });
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPasswordEmailSent(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(Guid id, string code)
        {
            var model = new ResetPasswordViewModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(Guid id, string code, ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await client.Registration.ResetPasswordAsync(new PasswordResetData
                    {
                        Password = model.Password,
                        Token = code,
                        UserId = id
                    });

                    return RedirectToAction("PasswordUpdated", "Account");
                }
                catch (ApiBadRequestException ex)
                {
                    this.HandleBadRequest(ex);

                    if (ModelState.IsValid)
                    {
                        throw;
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult PasswordUpdated()
        {
            return View();
        }
    }
}