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
    using Requests.Shared;
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

            return RedirectToAction("NotificationTypeQuestion",
                new { ca = model.CompetentAuthorities.SelectedValue });
        }

        [HttpGet]
        public ActionResult NotificationTypeQuestion(string ca, string nt)
        {
            var model = new InitialQuestionsViewModel
            {
                SelectedNotificationType = NotificationType.Recovery,
                CompetentAuthority = ca.GetValueFromDisplayName<CompetentAuthority>()
            };

            if (!string.IsNullOrWhiteSpace(nt))
            {
                model.SelectedNotificationType = nt.GetValueFromDisplayName<NotificationType>();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> NotificationTypeQuestion(InitialQuestionsViewModel model)
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
                                NotificationType = model.SelectedNotificationType
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

        private async Task<IEnumerable<CountryData>> GetCountries(IIwsClient iwsClient)
        {
            var response = await iwsClient.SendAsync(new GetCountries());

            if (response.HasErrors)
            {
                // TODO - error handling
            }

            return response.Result;
        }

        [HttpGet]
        public async Task<ActionResult> ProducerInformation(Guid id)
        {
            var model = new ProducerInformationViewModel();
            var address = new AddressViewModel { Countries = await GetCountries() };
            var business = new BusinessViewModel();

            model.NotificationId = id;
            model.AddressDetails = address;
            model.BusinessViewModel = business;

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ProducerInformation(ProducerInformationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AddressDetails.Countries = await GetCountries();
                return View(model);
            }

            var producerData = new CreateProducer
            {
                NotificationId = model.NotificationId,

                IsSiteOfExport = model.IsSiteOfExport,

                Name = model.BusinessViewModel.Name,
                Type = model.BusinessViewModel.EntityType,
                RegistrationNumber = model.BusinessViewModel.CompaniesHouseRegistrationNumber ??
                    (model.BusinessViewModel.SoleTraderRegistrationNumber ?? model.BusinessViewModel.PartnershipRegistrationNumber),
                AdditionalRegistrationNumber = model.BusinessViewModel.AdditionalRegistrationNumber,

                Building = model.AddressDetails.Building,
                Address1 = model.AddressDetails.Address1,
                Address2 = model.AddressDetails.Address2,
                TownOrCity = model.AddressDetails.TownOrCity,
                County = model.AddressDetails.County,
                PostalCode = model.AddressDetails.Postcode,
                CountryId = model.AddressDetails.CountryId,

                FirstName = model.ContactDetails.FirstName,
                LastName = model.ContactDetails.LastName,
                Phone = model.ContactDetails.Telephone,
                Fax = model.ContactDetails.Fax,
                Email = model.ContactDetails.Email,
            };

            using (var client = apiClient())
            {
                var response = await client.SendAsync(User.GetAccessToken(), producerData);

                if (response.HasErrors)
                {
                    this.AddValidationErrorsToModelState(response);
                    model.AddressDetails.Countries = await GetCountries(client);
                    return View(model);
                }
            }

            return RedirectToAction("MultipleProducers", "NotificationApplication", new { notificationId = model.NotificationId });
        }

        [HttpGet]
        public async Task<ActionResult> ExporterNotifier(Guid id)
        {
            ExporterNotifier model = new ExporterNotifier();
            var address = new AddressViewModel { Countries = await GetCountries() };
            var business = new BusinessViewModel();

            model.NotificationId = id;
            model.AddressDetails = address;
            model.BusinessViewModel = business;

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ExporterNotifier(ExporterNotifier model)
        {
            if (!ModelState.IsValid)
            {
                model.AddressDetails.Countries = await GetCountries();
                return View(model);
            }

            using (var client = apiClient())
            {
                var response = await client.SendAsync(User.GetAccessToken(), new CreateExporter()
                {
                    Name = model.BusinessViewModel.Name,
                    Type = model.BusinessViewModel.EntityType,
                    RegistrationNumber = model.BusinessViewModel.CompaniesHouseRegistrationNumber ?? (model.BusinessViewModel.SoleTraderRegistrationNumber ?? model.BusinessViewModel.PartnershipRegistrationNumber),
                    AdditionalRegistrationNumber = model.BusinessViewModel.AdditionalRegistrationNumber,
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
                    model.AddressDetails.Countries = await GetCountries(client);
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

        [HttpGet]
        public async Task<ActionResult> MultipleProducers(Guid notificationId, string errorMessage = "")
        {
            var model = new MultipleProducersViewModel();

            if (!String.IsNullOrEmpty(errorMessage))
            {
                ModelState.AddModelError(string.Empty, errorMessage);
            }

            using (var client = apiClient())
            {
                var response = await client.SendAsync(User.GetAccessToken(), new GetProducersByNotificationId(notificationId));

                if (response.HasErrors)
                {
                    this.AddValidationErrorsToModelState(response);
                    return View(model);
                }
                model.NotificationId = notificationId;
                model.ProducerData = response.Result.ToList();
                model.IsSiteOfExportSet = model.ProducerData.Exists(p => p.IsSiteOfExport);
            }
            return View("MultipleProducers", model);
        }

        [HttpPost]
        public async Task<ActionResult> MultipleProducers(MultipleProducersViewModel model)
        {
            if (!model.IsSiteOfExportSet)
            {
                return RedirectToAction("MultipleProducers", "NotificationApplication",
                    new
                    {
                        notificationId = model.NotificationId,
                        errorMessage = "Please select a site of export"
                    });
            }
            return RedirectToAction("ExporterNotifier", "NotificationApplication", new { id = model.NotificationId });
        }

        [HttpGet]
        public ActionResult ShowConfirmDelete(Guid producerId, Guid notificationId, bool isSiteOfExport)
        {
            var model = new ProducerData
            {
                ProducerId = producerId,
                NotificationId = notificationId,
                IsSiteOfExport = isSiteOfExport
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView("ConfirmDeleteProducer", model);
            }
            return View("ConfirmDeleteProducer", model);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteProducer(ProducerData model)
        {
            if (model.IsSiteOfExport)
            {
                return RedirectToAction("MultipleProducers", "NotificationApplication",
                    new
                    {
                        notificationId = model.NotificationId,
                        errorMessage = "Please make another producer the site of export before you delete this producer"
                    });
            }
            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(), new DeleteProducer(model.ProducerId, model.NotificationId));
            }
            return RedirectToAction("MultipleProducers", "NotificationApplication", new { notificationId = model.NotificationId });
        }
    }
}