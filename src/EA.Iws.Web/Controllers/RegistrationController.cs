﻿namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Api.Client.Entities;
    using Core.Registration;
    using Infrastructure;
    using Microsoft.Owin.Security;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Prsd.Core.Web.OAuth;
    using Requests.Organisations;
    using Requests.Registration;
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
        public async Task<ActionResult> ApplicantRegistration()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home", new { area = string.Empty });
            }

            var tokenResponse = await oauthClientCredentialClient().GetClientCredentialsAsync();

            var model = new ApplicantRegistrationViewModel();
            await this.BindCountryList(client, tokenResponse.AccessToken);
            model.Address.DefaultCountryId = this.GetDefaultCountryId();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ApplicantRegistration(ApplicantRegistrationViewModel model)
        {
            var response = await oauthClientCredentialClient().GetClientCredentialsAsync();

            if (ModelState.IsValid)
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
                    var userId = await client.Registration.RegisterApplicantAsync(response.AccessToken, applicantRegistrationData);
                    var signInResponse = await oauthClient().GetAccessTokenAsync(model.Email, model.Password);
                    authenticationManager.SignIn(signInResponse.GenerateUserIdentity());

                    var emailSent =
                        await
                            client.Registration.SendEmailVerificationAsync(signInResponse.AccessToken,
                                new EmailVerificationData
                                {
                                    Url = Url.Action("VerifyEmail", "Account", null, Request.Url.Scheme)
                                });

                    var addressId =
                        await
                            client.SendAsync(signInResponse.AccessToken,
                                new CreateAddress { Address = model.Address, UserId = userId });
                    applicantRegistrationData.AddressId = addressId;

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

                await this.BindCountryList(client, response.AccessToken);
                return View(model);
            }

            await this.BindCountryList(client, response.AccessToken);
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
                var response =
                    await client.SendAsync(User.GetAccessToken(), new FindMatchingOrganisations(organisationName));

                if (response == null || response.Count <= 0)
                {
                    return RedirectToAction("CreateNewOrganisation", new { organisationName = model.Name });
                }

                model.Organisations = response;
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

        [HttpGet]
        public ActionResult CreateNewOrganisation(string organisationName)
        {
            var model = new CreateNewOrganisationViewModel { Name = organisationName };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateNewOrganisation(CreateNewOrganisationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var organisationRegistrationData = new OrganisationRegistrationData
            {
                Name = model.Name,
                BusinessType = model.BusinessType,
                OtherDescription = model.OtherDescription
            };

            try
            {
                var organisationId =
                    await
                        client.SendAsync(User.GetAccessToken(), new CreateOrganisation(organisationRegistrationData));
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

            return View(model);
        }

        [HttpGet]
        public ActionResult ChangeAccountDetails()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeAccountDetails(ChangeAccountDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.ChangeOptions.GetValueOrDefault())
            {
                return RedirectToAction("EditOrganisationDetails", "Registration");
            }
            return RedirectToAction("EditApplicantDetails", "Registration");
        }

        [HttpGet]
        public async Task<ActionResult> EditApplicantDetails()
        {
            EditApplicantDetailsViewModel model;

            var response = await client.Registration.GetApplicantDetailsAsync(User.GetAccessToken());
            model = new EditApplicantDetailsViewModel(response);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditApplicantDetails(EditApplicantDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                if (model.Email.Equals(model.ExistingEmail))
                {
                    //Update applicant details only
                    await client.Registration.UpdateApplicantDetailsAsync(User.GetAccessToken(), model.ToRequest());
                    return RedirectToAction("Home", "Applicant", new { id = model.Id, area = string.Empty });
                }

                //Update applicant details & Send verification email
                await client.Registration.UpdateApplicantDetailsAsync(User.GetAccessToken(), model.ToRequest());

                var emailSent =
                    await
                        client.Registration.SendEmailVerificationAsync(User.GetAccessToken(),
                            new EmailVerificationData
                            {
                                Url = Url.Action("VerifyEmail", "Account", null, Request.Url.Scheme)
                            });

                return RedirectToAction("EmailVerificationRequired", "Account");
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

        [HttpGet]
        public async Task<ActionResult> EditOrganisationDetails()
        {
            var response = await client.SendAsync(User.GetAccessToken(), new GetOrganisationDetailsByUser());
            var model = new EditOrganisationViewModel(response);

            await this.BindCountryList(client, User.GetAccessToken());
            model.CountryId =
                new Guid(
                    ((SelectList)ViewBag.Countries).Single(
                        c =>
                            c.Text.Equals(response.Address.CountryName, StringComparison.InvariantCultureIgnoreCase))
                        .Value);
            model.DefaultCountryId = this.GetDefaultCountryId();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditOrganisationDetails(EditOrganisationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(client, User.GetAccessToken());
                return View(model);
            }

            await client.SendAsync(User.GetAccessToken(), new UpdateOrganisationDetails(model.ToRequest()));

            return RedirectToAction("Home", "Applicant");
        }
    }
}