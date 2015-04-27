namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Api.Client.Entities;
    using Services;
    using ViewModels.Registration;

    public class RegistrationController : Controller
    {
        private readonly AppConfiguration config;

        public RegistrationController(AppConfiguration config)
        {
            this.config = config;
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
                    var userDetails = new ApplicantRegistrationData
                    {
                        Email = model.Email,
                        FirstName = model.Name,
                        Surname = model.Surname,
                        Phone = model.PhoneNumber,
                        Password = model.Password
                    };

                    response = await client.ApplicantRegistration.RegisterApplicantAsync("Test", userDetails);
                }

                if (response.IsSuccessStatusCode)
                {
                    return View("ApplicantRegistration", model);
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
        public ActionResult SelectOrganisation(string name)
        {
            var model = new SelectOrganisationViewModel
            {
                Name = name,
                Organisations = GetTestOrganisations()
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
    }
}