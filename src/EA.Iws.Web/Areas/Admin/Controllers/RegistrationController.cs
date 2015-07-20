namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
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
        [AllowAnonymous]
        public ActionResult Register()
        {
            var model = GetModelData(new AdminRegistrationViewModel());
            return View(model);
        }

        private AdminRegistrationViewModel GetModelData(AdminRegistrationViewModel model)
        {
            var competentAuthorities = new List<SelectListItem>
            {
                new SelectListItem{Text = "EA", Value = "EA"},
                new SelectListItem{Text = "SEPA", Value = "SEPA"},
                new SelectListItem{Text = "NIEA", Value = "NIEA"},
                new SelectListItem{Text = "NRW", Value = "NRW"},
            };

            var areas = new List<SelectListItem>
            {
                new SelectListItem{Text = "North", Value = "North"},
                new SelectListItem{Text = "South", Value = "South"},
                new SelectListItem{Text = "East", Value = "East"},
                new SelectListItem{Text = "West", Value = "West"},
            };

            model.CompetentAuthorities = new SelectList(competentAuthorities, "Text", "Value");
            model.Areas = new SelectList(areas, "Text", "Value");
            return model;
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(AdminRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(GetModelData(model));
            }

            using (var client = apiClient())
            {
                var adminRegistrationData = new AdminRegistrationData
                {
                    Email = model.Email,
                    FirstName = model.Name,
                    Surname = model.Surname,
                    Password = model.Password,
                    ConfirmPassword = model.ConfirmPassword,
                    LocalArea = model.LocalArea,
                    CompetentAuthority = model.CompetentAuthority,
                    JobTitle = model.JobTitle
                };

                try
                {
                    await client.Registration.RegisterAdminAsync(adminRegistrationData);
                    var signInResponse = await oauthClient().GetAccessTokenAsync(model.Email, model.Password);
                    authenticationManager.SignIn(signInResponse.GenerateUserIdentity());

                    return View(GetModelData(model));
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
            return View(GetModelData(model));
        }
    }
}