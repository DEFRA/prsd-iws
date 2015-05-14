namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core;
    using Prsd.Core.Extensions;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Notification;
    using Requests.Organisations;
    using Requests.Registration;
    using ViewModels.NotificationApplication;
    using ViewModels.Shared;

    [Authorize]
    public class NotificationApplicationController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public NotificationApplicationController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public ActionResult CompetentAuthority()
        {
            var model = new CompetentAuthorityChoiceViewModel
            {
                CompetentAuthorities =
                    RadioButtonStringCollectionViewModel.CreateFromEnum<CompetentAuthority>()
            };

            return View("CompetentAuthority", model);
        }

        [HttpPost]
        public ActionResult CompetentAuthority(CompetentAuthorityChoiceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("CompetentAuthority", model);
            }

            return RedirectToAction("WasteActionQuestion",
                new { ca = model.CompetentAuthorities.SelectedValue });
        }

        [HttpGet]
        public ActionResult WasteActionQuestion(string ca, string nt)
        {
            var model = new InitialQuestionsViewModel
            {
                SelectedWasteAction = WasteAction.Recovery,
                CompetentAuthority = ca.GetValueFromDisplayName<CompetentAuthority>()
            };

            if (!string.IsNullOrWhiteSpace(nt))
            {
                model.SelectedWasteAction = nt.GetValueFromDisplayName<WasteAction>();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> WasteActionQuestion(InitialQuestionsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                var response =
                    await
                        client.SendAsync(User.GetAccessToken(),
                            new CreateNotificationApplication
                            {
                                CompetentAuthority = model.CompetentAuthority,
                                WasteAction = model.SelectedWasteAction
                            });

                if (!response.HasErrors)
                {
                    return RedirectToAction("Created",
                        new
                        {
                            id = response.Result
                        });
                }
                this.AddValidationErrorsToModelState(response);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Created(Guid id)
        {
            using (var client = apiClient())
            {
                var response = await client.SendAsync(User.GetAccessToken(), new GetNotificationNumber(id));

                if (response.HasErrors)
                {
                    // TODO - error handling
                }

                var model = new CreatedViewModel
                {
                    NotificationId = id,
                    NotificationNumber = response.Result
                };
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Created(CreatedViewModel model)
        {
            return RedirectToAction(actionName: "ExporterNotifier", controllerName: "NotificationApplication", routeValues: new { id = model.NotificationId });
        }

        public async Task<ActionResult> GenerateNotificationDocument(Guid id)
        {
            using (var client = apiClient())
            {
                var response =
                    await client.SendAsync(User.GetAccessToken(), new GenerateNotificationDocument(id));

                if (response.HasErrors)
                {
                    return HttpNotFound(response.Errors.FirstOrDefault());
                }

                var downloadName = "IwsNotification" + SystemTime.UtcNow + ".docx";

                return File(response.Result, Prsd.Core.Web.Constants.MicrosoftWordContentType, downloadName);
            }
        }

        [HttpGet]
        public async Task<ActionResult> ProducerInformation(Guid id)
        {
            var model = new ProducerInformationViewModel();
            var address = new AddressViewModel { Countries = await GetCountries() };

            model.NotificationId = id;
            model.AddressDetails = address;

            return View(model);
        }

        private async Task<IEnumerable<CountryData>> GetCountries()
        {
            using (var client = apiClient())
            {
                var response = await client.SendAsync(new GetCountries());

                if (response.HasErrors)
                {
                    // TODO - error handling
                }

                return response.Result;
            }
        }

        [HttpPost]
        public async Task<ActionResult> ProducerInformation(ProducerInformationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var countries = await GetCountries();
                model.AddressDetails.Countries = countries;
                return View(model);
            }

            var address = new AddressData
            {
                Building = model.AddressDetails.Building,
                StreetOrSuburb = model.AddressDetails.Address1,
                Address2 = model.AddressDetails.Address2,
                TownOrCity = model.AddressDetails.TownOrCity,
                Region = model.AddressDetails.County,
                PostalCode = model.AddressDetails.Postcode,
                CountryId = model.AddressDetails.CountryId
            };

            var contact = new ContactData
            {
                FirstName = model.ContactDetails.FirstName,
                LastName = model.ContactDetails.LastName,
                Telephone = model.ContactDetails.Telephone,
                Fax = model.ContactDetails.Fax,
                Email = model.ContactDetails.Email
            };

            var producerData = new CreateProducer
            {
                Name = model.Name,
                IsSiteOfExport = model.IsSiteOfExport,
                Type = model.EntityType,
                CompaniesHouseNumber = model.CompaniesHouseReference,
                RegistrationNumber1 = model.SoleTraderRegistrationNumber ?? model.PartnershipRegistrationNumber,
                RegistrationNumber2 = model.RegistrationNumber2,
                Address = address,
                Contact = contact,
                NotificationId = model.NotificationId
            };

            using (var client = apiClient())
            {
                var response = await client.SendAsync(User.GetAccessToken(), producerData);

                if (response.HasErrors)
                {
                    this.AddValidationErrorsToModelState(response);
                    return View(model);
                }
            }

            return RedirectToAction("Index", "Home");
        }
        
        [HttpGet]
        public async Task<ActionResult> ExporterNotifier(Guid id)
        {
            ExporterNotifier model = new ExporterNotifier();
            model.NotificationId = id;
            var address = new AddressViewModel { Countries = await GetCountries() };
            model.AddressDetails = address;
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ExporterNotifier(ExporterNotifier model)
        {
            using (var client = apiClient())
            {
                var response = await client.SendAsync(User.GetAccessToken(), new CreateExporter()
                {
                    Name = model.Name,
                    Type = model.EntityType,
                    CompanyHouseNumber = model.CompaniesHouseReference,
                    RegistrationNumber1 = model.SoleTraderRegistrationNumber ?? model.PartnershipRegistrationNumber,
                    RegistrationNumber2 = model.RegistrationNumber2,
                    NotificationId = model.NotificationId,
                    Building = model.AddressDetails.Building,
                    Address1 = model.AddressDetails.Address1,
                    Address2 = model.AddressDetails.Address2,
                    City = model.AddressDetails.TownOrCity,
                    County = model.AddressDetails.County,
                    PostCode = model.AddressDetails.Postcode,
                    CountryId = model.AddressDetails.CountryId,
                    FirstName = model.ContactDetails.FirstName,
                    LastName = model.ContactDetails.LastName,
                    Phone = model.ContactDetails.Telephone,
                    Email = model.ContactDetails.Email,
                    Fax = model.ContactDetails.Fax
                });

                if (response.HasErrors)
                {
                    this.AddValidationErrorsToModelState(response);
                    model.AddressDetails.Countries = await GetCountries();
                    return View(model);
                }
            }

            return RedirectToAction(actionName: "ProducerInformation", controllerName: "NotificationApplication", routeValues: new { id = model.NotificationId });
        }

        public ActionResult _GetUserNotifications()
        {
            using (var client = apiClient())
            {
                // Child actions (partial views) cannot be async and we must therefore get the result of the task.
                // The called code must use ConfigureAwait(false) on async tasks to prevent deadlock.
                var response =
                    client.SendAsync(User.GetAccessToken(), new GetNotificationsByUser()).Result;

                if (response.HasErrors)
                {
                    ViewBag.Errors = response.Errors;
                    return PartialView(null);
                }

                return PartialView(response.Result);
            }
        }
    }
}