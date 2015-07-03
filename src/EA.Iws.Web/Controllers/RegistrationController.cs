namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Api.Client.Entities;
    using Core.Registration;
    using Core.Shared;
    using Infrastructure;
    using Microsoft.Owin.Security;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Prsd.Core.Web.OAuth;
    using Requests.Organisations;
    using Requests.Registration;
    using Requests.Shared;
    using Services;
    using ViewModels.Registration;

    [Authorize]
    public class RegistrationController : Controller
    {
        private readonly Func<IIwsClient> apiClient;
        private readonly IAuthenticationManager authenticationManager;
        private readonly Func<IOAuthClient> oauthClient;
        private readonly IEmailService emailService;

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
        public ActionResult ApplicantRegistration()
        {
            return View(new ApplicantRegistrationViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ApplicantRegistration(ApplicantRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var client = apiClient())
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

                        return RedirectToAction("SelectOrganisation", new { organisationName = model.OrganisationName });
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

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> SelectOrganisation(string organisationName)
        {
            var model = new SelectOrganisationViewModel
            {
                Name = organisationName
            };

            if (string.IsNullOrEmpty(organisationName))
            {
                model.Organisations = null;
            }
            else
            {
                using (var client = apiClient())
                {
                    var response =
                        await client.SendAsync(User.GetAccessToken(), new FindMatchingOrganisations(organisationName));

                    if (response == null || response.Count <= 0)
                    {
                        return RedirectToAction("CreateNewOrganisation", new { organisationName = model.Name });
                    }

                    model.Organisations = response;
                }
            }

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SelectOrganisation(SelectOrganisationViewModel model, string submitButton)
        {
            Guid selectedGuid;
            if (!Guid.TryParse(submitButton, out selectedGuid) ||
                model.Organisations.SingleOrDefault(o => o.Id == selectedGuid) == null)
            {
                return RedirectToAction("SelectOrganisation", new { organisationName = model.Name });
            }

            using (var client = apiClient())
            {
                try
                {
                    var response =
                        await client.SendAsync(User.GetAccessToken(), new LinkUserToOrganisation(selectedGuid));

                    if (response)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (ApiBadRequestException ex)
                {
                    this.HandleBadRequest(ex);

                    if (ModelState.IsValid)
                    {
                        throw;
                    }
                }

                return RedirectToAction("SelectOrganisation", new { organisationName = model.Name });
            }
        }

        [HttpGet]
        public async Task<ActionResult> CreateNewOrganisation(string organisationName)
        {
            var model = new CreateNewOrganisationViewModel { Name = organisationName, Countries = await GetCountries() };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateNewOrganisation(CreateNewOrganisationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Countries = await GetCountries();
                return View(model);
            }

            var organisationRegistrationData = new OrganisationRegistrationData
            {
                Name = model.Name,
                Building = model.Building,
                Address1 = model.Address1,
                Address2 = model.Address2,
                TownOrCity = model.TownOrCity,
                Postcode = model.Postcode,
                CountryId = model.CountryId,
                BusinessType = model.BusinessType,
                OtherDescription = model.OtherDescription
            };

            using (var client = apiClient())
            {
                try
                {
                    var organisationId = await client.SendAsync(User.GetAccessToken(), new CreateOrganisation(organisationRegistrationData));
                    await client.SendAsync(User.GetAccessToken(), new LinkUserToOrganisation(organisationId));
                    return RedirectToAction("Home", "Applicant");
                }
                catch (ApiBadRequestException ex)
                {
                    this.HandleBadRequest(ex);

                    if (ModelState.IsValid)
                    {
                        throw;
                    }
                }
                model.Countries = await GetCountries();
                return View(model);
            }
        }

        private async Task<IEnumerable<CountryData>> GetCountries()
        {
            using (var client = apiClient())
            {
                return await client.SendAsync(new GetCountries());
            }
        }
    }
}