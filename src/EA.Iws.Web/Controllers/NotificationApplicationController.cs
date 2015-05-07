namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Api.Client.Entities;
    using Infrastructure;
    using Services;
    using Utils;
    using ViewModels.NotificationApplication;
    using ViewModels.Shared;

    [Authorize]
    public class NotificationApplicationController : Controller
    {
        private readonly Func<IIwsClient> apiClient;
        private readonly AppConfiguration config;

        public NotificationApplicationController(AppConfiguration config, Func<IIwsClient> apiClient)
        {
            this.config = config;
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
            InitialQuestionsViewModel model = new InitialQuestionsViewModel
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
                var response = await client.Notification.CreateNotificationApplicationAsync(User.GetAccessToken(),
                    model.ToNotificationData());

                if (!response.HasErrors)
                {
                    return RedirectToAction("Created",
                        new
                        {
                            id = response.Result
                        });
                }
                else
                {
                    this.AddValidationErrorsToModelState(response);
                    return View(model);
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult> Created(Guid id)
        {
            using (var client = apiClient())
            {
                var notification = await client.Notification.GetNotificationInformationAsync(User.GetAccessToken(), id);
                var model = new CreatedViewModel()
                {
                    NotificationId = notification.Id,
                    NotificationNumber = notification.NotificationNumber
                };
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Created()
        {
            return RedirectToAction(actionName: "Home", controllerName: "Applicant");
        }

        public async Task<ActionResult> GenerateNotificationDocument(Guid id)
        {
            using (var client = apiClient())
            {
                Response<byte[]> response =
                    await client.Notification.GenerateNotificationDocumentAsync(User.GetAccessToken(), id);

                if (response.HasErrors)
                {
                    return HttpNotFound(response.Errors.FirstOrDefault());
                }

                var downloadName = "IwsNotification" + SystemTime.UtcNow + ".docx";

                return File(response.Result, Constants.MicrosoftWordContentType, downloadName);
            }
        }

        [HttpGet]
        public ActionResult ProducerInformation(Guid id)
        {
            var model = new ProducerInformationViewModel();

            model.NotificationId = id;
        
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ProducerInformation(ProducerInformationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var address = new AddressData();
            address.Building = model.AddressDetails.Building;
            address.StreetOrSuburb = model.AddressDetails.Address1;
            address.Address2 = model.AddressDetails.Address2;
            address.TownOrCity = model.AddressDetails.TownOrCity;
            address.Region = model.AddressDetails.County;
            address.PostalCode = model.AddressDetails.Postcode;

            var contact = new ContactData();
            contact.FirstName = model.ContactDetails.FirstName;
            contact.LastName = model.ContactDetails.LastName;
            contact.Telephone = model.ContactDetails.Telephone;
            contact.Fax = model.ContactDetails.Fax;
            contact.Email = model.ContactDetails.Email;

            var producerData = new ProducerData();
            producerData.Name = model.Name;
            producerData.IsSiteOfExport = model.IsSiteOfExport;
            producerData.Type = model.EntityType;
            producerData.CompaniesHouseNumber = model.CompaniesHouseReference;
            producerData.RegistrationNumber1 = model.SoleTraderRegistrationNumber ?? model.PartnershipRegistrationNumber;
            producerData.RegistrationNumber2 = model.RegistrationNumber2;
            producerData.Address = address;
            producerData.Contact = contact;
            producerData.NotificationId = model.NotificationId;

            using (var client = apiClient())
            {
                var response = await client.Producer.CreateProducer(User.GetAccessToken(), producerData);

                if (response.HasErrors)
                {
                    this.AddValidationErrorsToModelState(response);
                    return View(model);
                }
            }

            return RedirectToAction("Index", "Home");  // Change to point to correct view
        }

        public ActionResult _GetUserNotifications()
        {
            NotificationApplicationSummaryData[] model = null;

            using (var client = apiClient())
            {
                // Child actions (partial views) cannot be async and we must therefore get the result of the task.
                // The called code must use ConfigureAwait(false) on async tasks to prevent deadlock.
                Response<NotificationApplicationSummaryData[]> response =
                    client.Notification.GetUserNotifications(User.GetAccessToken()).Result;

                if (response.HasErrors)
                {
                    ViewBag.Errors = response.Errors;
                    return PartialView(model);
                }

                return PartialView(response.Result);
            }
        }
    }
}