namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Api.Client.Entities;
    using Infrastructure;
    using Microsoft.Owin.Security;
    using Services;
    using ViewModels.Registration;

    [Authorize]
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
        [Authorize]
        public async Task<ActionResult> SelectOrganisation(string organisationName)
        {
            var model = new SelectOrganisationViewModel
            {
                Name = organisationName
            };

            using (var client = new IwsClient(config.ApiUrl))
            {
                if (string.IsNullOrEmpty(organisationName))
                {
                    model.Organisations = null;
                }
                else
                {
                    model.Organisations = await client.Registration.SearchOrganisationAsync(User.GetAccessToken(), organisationName);
                }
            }

            return View("SelectOrganisation", model);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> SelectOrganisation(SelectOrganisationViewModel model, string submitButton)
        {
            Guid selectedGuid;
            if (!Guid.TryParse(submitButton, out selectedGuid) || model.Organisations.SingleOrDefault(o => o.Id == selectedGuid) == null)
            {
                return RedirectToAction("SelectOrganisation", routeValues: new { organisationName = model.Name });
            }

            using (var client = new IwsClient(config.ApiUrl))
            {
                try
                {
                    await client.Registration.LinkUserToOrganisationAsync(User.GetAccessToken(), selectedGuid);
                }
                catch (Exception)
                {
                    return RedirectToAction("SelectOrganisation", routeValues: new { organisationName = model.Name });
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> CreateNewOrganisation(string organisationName)
        {
            var model = new CreateNewOrganisationViewModel { Name = organisationName, Countries = await GetCountries() };
            return View("CreateNewOrganisation", model);
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewOrganisation(CreateNewOrganisationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Countries = await GetCountries();
                return View("CreateNewOrganisation", model);
            }

            HttpResponseMessage response;
            var organisationRegistrationData = new OrganisationRegistrationData
            {
                Name = model.Name,
                Address1 = model.Address1,
                Address2 = model.Address2,
                TownOrCity = model.Address3,
                Postcode = model.Postcode,
                Country = model.Country,
                EntityType = model.EntityType,
                CompaniesHouseNumber = model.CompaniesHouseReference
            };

            using (var client = new IwsClient(config.ApiUrl))
            {
                response = await client.Registration.RegisterOrganisationAsync(User.GetAccessToken(), organisationRegistrationData);
            }

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("CreateNewOrganisation", "Registration", new { organisationName = model.Name });
        }

        private async Task<IEnumerable<CountryData>> GetCountries()
        {
            using (var client = new IwsClient(config.ApiUrl))
            {
                return await client.Registration.GetCountriesAsync();
            }
        }
    }
}