namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Api.Client.Entities;
    using ControllerHelpers;
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
                    await LoginHelper.LogIn(model.Email, model.Password, oauthClient, authenticationManager);

                    return RedirectToAction("SelectOrganisation", new { organisationName = model.OrganisationName });
                }
                this.AddValidationErrorsToModelState(response);
            }
            return View("ApplicantRegistration", model);
        }

        [HttpGet]
        public ActionResult OrganisationGrid(IList<OrganisationViewModel> model)
        {
            return PartialView("_OrganisationGrid", model);
        }

        [HttpGet]
        public ActionResult SelectOrganisation(string organisationName)
        {
            var model = new SelectOrganisationViewModel
            {
                Name = organisationName,
                Organisations = null
            };

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

        private IList<OrganisationViewModel> GetTestOrganisations()
        {
            return new[]
            {
                new OrganisationViewModel
                {
                    Id = Guid.NewGuid(),
                    Postcode = "GU22 7mx",
                    TownOrCity = "Woking",
                    Name = "SFW Ltd"
                },
                new OrganisationViewModel
                {
                    Id = Guid.NewGuid(),
                    Postcode = "GU22 7UY",
                    TownOrCity = "Woking",
                    Name = "SWF Plumbing"
                }
            };
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