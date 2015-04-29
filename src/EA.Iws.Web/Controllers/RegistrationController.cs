namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Api.Client.Entities;
    using Infrastructure;
    using Microsoft.Owin.Security;
    using Services;
    using ViewModels.Registration;

    public class RegistrationController : Controller
    {
        private readonly IAuthenticationManager authenticationManager;
        private readonly AppConfiguration config;
        private readonly Func<IwsOAuthClient> oauthClient;

        public RegistrationController(AppConfiguration config, Func<IwsOAuthClient> oauthClient,
            IAuthenticationManager authenticationManager)
        {
            this.config = config;
            this.oauthClient = oauthClient;
            this.authenticationManager = authenticationManager;
        }

        public ActionResult ApplicantRegistration()
        {
            return View(new ApplicantRegistrationViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitApplicantRegistration(ApplicantRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response;
                using (var client = new IwsClient(config.ApiUrl))
                {
                    var applicantRegistrationData = new ApplicantRegistrationData
                    {
                        Email = model.Email,
                        FirstName = model.Name,
                        Surname = model.Surname,
                        Phone = model.PhoneNumber,
                        Password = model.Password,
                        ConfirmPassword = model.ConfirmPassword
                    };

                    response = await client.Registration.RegisterApplicantAsync(applicantRegistrationData);
                }

                if (response.IsSuccessStatusCode)
                {
                    var signInResponse = await oauthClient().GetAccessTokenAsync(model.Email, model.Password);
                    authenticationManager.SignIn(signInResponse.GenerateUserIdentity());

                    return RedirectToAction("SelectOrganisation", new { organisationName = model.OrganisationName });
                }
                this.AddValidationErrorsToModelState(response);
            }
            return View("ApplicantRegistration", model);
        }

        [HttpGet]
        public ActionResult OrganisationGrid(IList<OrganisationData> model)
        {
            return PartialView("_OrganisationGrid", model);
        }

        [HttpGet]
        public async Task<ActionResult> SelectOrganisation(string organisationName)
        {
            var model = new SelectOrganisationViewModel
            {
                Name = organisationName
            };

            using (var client = new IwsClient(config.ApiUrl))
            {
                model.Organisations = await client.Registration.SearchOrganisationAsync(organisationName);
            }

            return View("SelectOrganisation", model);
        }

        [HttpPost]
        public ActionResult SelectOrganisation(SelectOrganisationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("SelectOrganisation", model);
            }

            return RedirectToAction("SelectOrganisation");
        }

        [HttpGet]
        public ActionResult CreateNewOrganisation(string organisationName)
        {
            var model = new CreateNewOrganisationViewModel { Name = organisationName };

            return View("CreateNewOrganisation", model);
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewOrganisation(CreateNewOrganisationViewModel model)
        {
            HttpResponseMessage response;
            var organisationRegistrationData = new OrganisationRegistrationData();

            using (var client = new IwsClient(config.ApiUrl))
            {
                response =
                    await
                        client.Registration.RegisterOrganisationAsync(User.GetAccessToken(),
                            organisationRegistrationData);
            }

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("SelectOrganisation");
            }

            return View("CreateNewOrganisation", model);
        }
    }
}