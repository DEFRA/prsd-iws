namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System;
    using System.Linq;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Api.Client.Entities;
    using Core.Notification;
    using Infrastructure;
    using Microsoft.Owin.Security;
    using Prsd.Core.Helpers;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Prsd.Core.Web.OAuth;
    using Requests.Admin;
    using Services;
    using ViewModels;

    public class RegistrationController : Controller
    {
        private readonly Func<IIwsClient> apiClient;
        private readonly IAuthenticationManager authenticationManager;
        private readonly IEmailService emailService;
        private readonly Func<IOAuthClient> oauthClient;

        public RegistrationController(Func<IOAuthClient> oauthClient,
            Func<IIwsClient> apiClient,
            IAuthenticationManager authenticationManager,
            IEmailService emailService)
        {
            this.oauthClient = oauthClient;
            this.apiClient = apiClient;
            this.authenticationManager = authenticationManager;
            this.emailService = emailService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Register()
        {
            var model = await GetModelData(new AdminRegistrationViewModel());
            return View(model);
        }

        private async Task<AdminRegistrationViewModel> GetModelData(AdminRegistrationViewModel model)
        {
            var competentAuthorities = EnumHelper.GetValues(typeof(CompetentAuthority))
                .Select(p => new SelectListItem() { Text = p.Value, Value = p.Key.ToString() });

            model.CompetentAuthorities = new SelectList(competentAuthorities, "Value", "Text");
            
            using (var client = apiClient())
            {
                var result = await client.SendAsync(User.GetAccessToken(), new GetLocalAreas());
                model.Areas = new SelectList(result.Select(area => new SelectListItem { Text = area.Name, Value = area.Id.ToString() }), "Value", "Text");
            }
            
            return model;
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(AdminRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(await GetModelData(model));
            }

            using (var client = apiClient())
            {
                var adminRegistrationData = new AdminRegistrationData
                {
                    Email = model.Email,
                    FirstName = model.Name,
                    Surname = model.Surname,
                    Password = model.Password,
                    ConfirmPassword = model.ConfirmPassword
                };

                try
                {
                    var userId = await client.Registration.RegisterAdminAsync(adminRegistrationData);
                    var signInResponse = await oauthClient().GetAccessTokenAsync(model.Email, model.Password);
                    authenticationManager.SignIn(signInResponse.GenerateUserIdentity());

                    var verificationCode = await
                        client.Registration.GetUserEmailVerificationTokenAsync(signInResponse.AccessToken);
                    var verificationEmail = emailService.GenerateEmailVerificationMessage(Url.Action("AdminVerifyEmail", "Registration", null,
                        Request.Url.Scheme), verificationCode, userId, model.Email);
                    await emailService.SendAsync(verificationEmail);

                    await
                        client.SendAsync(signInResponse.AccessToken,
                            new CreateInternalUser(userId, model.JobTitle, model.LocalAreaId.GetValueOrDefault(), model.CompetentAuthority.GetValueOrDefault()));

                    return RedirectToAction("AdminEmailVerificationRequired");
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
            return View(await GetModelData(model));
        }

        [HttpGet]
        public ActionResult AdminEmailVerificationRequired()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AdminEmailVerificationRequired(FormCollection model)
        {
            try
            {
                using (var client = apiClient())
                {
                    var verificationToken = await client.Registration.GetUserEmailVerificationTokenAsync(User.GetAccessToken());
                    var verificationEmail =
                        emailService.GenerateEmailVerificationMessage(
                            Url.Action("AdminVerifyEmail", "Registration", null, Request.Url.Scheme),
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

            return RedirectToAction("AdminEmailVerificationRequired");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> AdminVerifyEmail(Guid id, string code)
        {
            using (var client = apiClient())
            {
                bool result = await client.Registration.VerifyEmailAsync(new VerifiedEmailData { Id = id, Code = code });

                if (!result)
                {
                    return RedirectToAction("AdminEmailVerificationRequired");
                }
            }

            return View();
        }
        
        [HttpGet]
        public ActionResult AwaitApproval()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ApprovalRejected()
        {
            return View();
        }
    }
}