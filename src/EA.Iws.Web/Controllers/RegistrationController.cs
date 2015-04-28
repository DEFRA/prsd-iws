namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Api.Client.Entities;
    using Infrastructure;
    using Microsoft.Owin.Security;
    using Services;
    using Thinktecture.IdentityModel.Client;
    using ViewModels.Registration;
    using TokenResponse = Api.Client.Entities.TokenResponse;

    public class RegistrationController : Controller
    {
        private readonly AppConfiguration config;
        private readonly IAuthenticationManager authenticationManager;
        private readonly Func<IwsOAuthClient> oauthClient;

        public RegistrationController(AppConfiguration config, Func<IwsOAuthClient> oauthClient, IAuthenticationManager authenticationManager)
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
                    //Registration is successful so sign the user in
                    var client = oauthClient();
                    var signInResponse = await client.GetAccessTokenAsync(model.Email, model.Password);
                    var identity = GenerateUserIdentity(signInResponse);
                    authenticationManager.SignIn(new AuthenticationProperties(), identity);

                    return RedirectToAction("SelectOrganisation", new { organisationName = model.OrganisationName });
                }
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
                response = await client.Registration.RegisterOrganisationAsync(User.GetAccessToken(), organisationRegistrationData);
            }

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("SelectOrganisation");
            }

            return View("CreateNewOrganisation", model);
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
    }
}