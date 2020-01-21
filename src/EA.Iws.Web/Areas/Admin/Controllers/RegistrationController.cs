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
    using Requests.Admin.UserAdministration;
    using ViewModels.Registration;

    [Authorize]
    public class RegistrationController : Controller
    {
        private readonly IIwsClient client;
        private readonly IAuthenticationManager authenticationManager;
        private readonly Func<IOAuthClient> oauthClient;
        private readonly Func<IOAuthClientCredentialClient> oauthClientCredentialClient;

        public RegistrationController(Func<IOAuthClient> oauthClient,
            IIwsClient client,
            IAuthenticationManager authenticationManager, 
            Func<IOAuthClientCredentialClient> oauthClientCredentialClient)
        {
            this.oauthClient = oauthClient;
            this.client = client;
            this.authenticationManager = authenticationManager;
            this.oauthClientCredentialClient = oauthClientCredentialClient;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home", new { area = string.Empty });
            }

            var model = await GetModelData(new AdminRegistrationViewModel());
            return View(model);
        }

        private async Task<AdminRegistrationViewModel> GetModelData(AdminRegistrationViewModel model)
        {
            var competentAuthorities = EnumHelper.GetValues(typeof(UKCompetentAuthority))
                .Select(p => new SelectListItem() { Text = p.Value, Value = p.Key.ToString() });

            model.CompetentAuthorities = new SelectList(competentAuthorities, "Value", "Text");

            string token = User.GetAccessToken();
            if (string.IsNullOrWhiteSpace(token))
            {
                var tokenResponse = await oauthClientCredentialClient().GetClientCredentialsAsync();

                token = tokenResponse.AccessToken;
            }

            var result = await client.SendAsync(token, new GetLocalAreas());

            model.Areas = new SelectList(result.Select(area => new SelectListItem { Text = area.Name, Value = area.Id.ToString() }), "Value", "Text");
            
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
                var response = await oauthClientCredentialClient().GetClientCredentialsAsync();

                var userId = await client.Registration.RegisterAdminAsync(response.AccessToken, adminRegistrationData);
                var signInResponse = await oauthClient().GetAccessTokenAsync(model.Email, model.Password);
                authenticationManager.SignIn(signInResponse.GenerateUserIdentity());

                var emailSent = await
                    client.Registration.SendEmailVerificationAsync(signInResponse.AccessToken,
                        new EmailVerificationData
                        {
                            Url = Url.Action("AdminVerifyEmail", "Registration", null, Request.Url.Scheme)
                        });

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
                var emailSent = await
                    client.Registration.SendEmailVerificationAsync(User.GetAccessToken(),
                        new EmailVerificationData
                        {
                            Url = Url.Action("AdminVerifyEmail", "Registration", null, Request.Url.Scheme)
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

            return RedirectToAction("AdminEmailVerificationRequired");
        }

        [HttpGet]
        public async Task<ActionResult> AdminVerifyEmail(Guid id, string code)
        {
            var response = await oauthClientCredentialClient().GetClientCredentialsAsync();

            var result = await client.Registration.VerifyEmailAsync(response.AccessToken, new VerifiedEmailData { Id = id, Code = code });

            if (!result)
            {
                return RedirectToAction("AdminEmailVerificationRequired");
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