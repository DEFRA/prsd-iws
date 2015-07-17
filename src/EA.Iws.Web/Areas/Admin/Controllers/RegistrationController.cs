namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Api.Client.Entities;
    using Infrastructure;
    using Microsoft.Owin.Security;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Prsd.Core.Web.OAuth;
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
        public ActionResult Register()
        {
            return View(new AdminRegistrationViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Register(AdminRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                var applicantRegistrationData = new ApplicantRegistrationData
                {
                    Email = model.Email,
                    FirstName = model.Name,
                    Surname = model.Surname,
                    Password = model.Password,
                    ConfirmPassword = model.ConfirmPassword,
                    Phone = "3333"
                };

                try
                {
                    var userId = await client.Registration.RegisterApplicantAsync(applicantRegistrationData);
                    var signInResponse = await oauthClient().GetAccessTokenAsync(model.Email, model.Password);
                    authenticationManager.SignIn(signInResponse.GenerateUserIdentity());

                    var verificationCode = await
                        client.Registration.GetUserEmailVerificationTokenAsync(signInResponse.AccessToken);
                    var verificationEmail = emailService.GenerateEmailVerificationMessage(Url.Action("VerifyEmail", "Account", null,
                        Request.Url.Scheme), verificationCode, userId, model.Email);
                    await emailService.SendAsync(verificationEmail);

                    return View();
                }
                catch (ApiBadRequestException ex)
                {
                    this.HandleBadRequest(ex);

                    if (ModelState.IsValid)
                    {
                        throw;
                    }
                }
                return View(model);
            }
        }
    }
}