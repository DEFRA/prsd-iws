namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using Core.ImportNotification.Summary;
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.Notification.AdditionalCharge;
    using EA.Iws.Core.SystemSettings;
    using EA.Iws.Requests.AdditionalCharge;
    using EA.Iws.Requests.ImportNotification;
    using EA.Iws.Requests.ImportNotification.Facilities;
    using EA.Iws.Requests.ImportNotification.Producers;
    using EA.Iws.Requests.SystemSettings;
    using EA.Iws.Web.Infrastructure.AdditionalCharge;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification.Exporters;
    using Requests.ImportNotification.Importers;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.EditContact;

    [AuthorizeActivity(typeof(SetExporterDetailsForImportNotification))]
    [AuthorizeActivity(typeof(SetImporterDetailsForImportNotification))]
    public class EditContactController : Controller
    {
        private readonly IMediator mediator;
        private readonly IAdditionalChargeService additionalChargeService;

        public EditContactController(IMediator mediator, IAdditionalChargeService additionalChargeService)
        {
            this.mediator = mediator;
            this.additionalChargeService = additionalChargeService;
        }

        [HttpGet]
        public async Task<ActionResult> Exporter(Guid id)
        {
            var exporter = await mediator.SendAsync(new GetExporterByImportNotificationId(id));
            var importNotificationDetails = await mediator.SendAsync(new GetNotificationDetails(id));
            if (importNotificationDetails != null)
            {
                exporter.CompetentAuthority = importNotificationDetails.CompetentAuthority;
                exporter.NotificationStatus = importNotificationDetails.Status;
                exporter.NotificationId = id;
            }
            var model = new EditContactViewModel(exporter);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Exporter(Guid id, EditContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var exporter = await mediator.SendAsync(new GetExporterByImportNotificationId(id));

            var newExporterData = new Exporter
            {
                Contact = GetNewContactData(model),
                Name = model.Name,
                Address = SetAddressData(model, exporter.Address)
            };

            await mediator.SendAsync(new SetExporterDetailsForImportNotification(id, newExporterData));

            if (model.AdditionalCharge != null)
            {
                if (model.AdditionalCharge.IsAdditionalChargesRequired.HasValue && model.AdditionalCharge.IsAdditionalChargesRequired.Value)
                {
                    var additionalCharge = new CreateImportNotificationAdditionalCharge(id, model.AdditionalCharge, AdditionalChargeType.EditExportDetails);

                    await additionalChargeService.AddImportAdditionalCharge(mediator, additionalCharge);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> Importer(Guid id)
        {
            var importer = await mediator.SendAsync(new GetImporterByImportNotificationId(id));
            var importNotificationDetails = await mediator.SendAsync(new GetNotificationDetails(id));
            if (importNotificationDetails != null)
            {
                importer.CompetentAuthority = importNotificationDetails.CompetentAuthority;
                importer.NotificationStatus = importNotificationDetails.Status;
                importer.NotificationId = id;
            }

            var model = new EditContactViewModel(importer);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Importer(Guid id, EditContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var importer = await mediator.SendAsync(new GetImporterByImportNotificationId(id));

            var newImporterData = new Importer
            {
                Contact = GetNewContactData(model),
                Name = model.Name,
                Address = SetAddressData(model, importer.Address)
            };

            await mediator.SendAsync(new SetImporterDetailsForImportNotification(id, newImporterData));

            if (model.AdditionalCharge != null)
            {
                if (model.AdditionalCharge.IsAdditionalChargesRequired.HasValue && model.AdditionalCharge.IsAdditionalChargesRequired.Value)
                {
                    var additionalCharge = new CreateImportNotificationAdditionalCharge(id, model.AdditionalCharge, AdditionalChargeType.EditImporterDetails);

                    await additionalChargeService.AddImportAdditionalCharge(mediator, additionalCharge);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> Producer(Guid id)
        {
            var producer = await mediator.SendAsync(new GetProducerByImportNotificationId(id));
            var importNotificationDetails = await mediator.SendAsync(new GetNotificationDetails(id));
            if (importNotificationDetails != null)
            {
                producer.CompetentAuthority = importNotificationDetails.CompetentAuthority;
                producer.NotificationStatus = importNotificationDetails.Status;
                producer.NotificationId = id;
            }
            var model = new EditContactViewModel(producer);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Producer(Guid id, EditContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var producer = await mediator.SendAsync(new GetProducerByImportNotificationId(id));

            var newProducerData = new Producer
            {
                Contact = GetNewContactData(model),
                Name = model.Name,
                Address = SetAddressData(model, producer.Address)
            };

            await mediator.SendAsync(new SetProducerDetailsForImportNotification(id, newProducerData));

            if (model.AdditionalCharge != null)
            {
                if (model.AdditionalCharge.IsAdditionalChargesRequired.HasValue && model.AdditionalCharge.IsAdditionalChargesRequired.Value)
                {
                    var additionalCharge = new CreateImportNotificationAdditionalCharge(id, model.AdditionalCharge, AdditionalChargeType.EditProducerDetails);

                    await additionalChargeService.AddImportAdditionalCharge(mediator, additionalCharge);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> Facility(Guid id)
        {
            var facility = await mediator.SendAsync(new GetFacilityByImportNotificationId(id));
            var importNotificationDetails = await mediator.SendAsync(new GetNotificationDetails(id));
            if (importNotificationDetails != null)
            {
                facility.CompetentAuthority = importNotificationDetails.CompetentAuthority;
                facility.NotificationStatus = importNotificationDetails.Status;
                facility.NotificationId = id;
            }
            var model = new EditContactViewModel(facility);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Facility(Guid id, EditContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var facitiy = await mediator.SendAsync(new GetFacilityByImportNotificationId(id));
            var newFacilityData = new Facility
            {
                Contact = GetNewContactData(model),
                Name = model.Name,
                Address = SetAddressData(model, facitiy.Address)
            };

            await mediator.SendAsync(new SetFacilityDetailsForImportNotification(id, newFacilityData));

            if (model.AdditionalCharge != null)
            {
                if (model.AdditionalCharge.IsAdditionalChargesRequired.HasValue && model.AdditionalCharge.IsAdditionalChargesRequired.Value)
                {
                    var additionalCharge = new CreateImportNotificationAdditionalCharge(id, model.AdditionalCharge, AdditionalChargeType.EditFacilityDetails);

                    await additionalChargeService.AddImportAdditionalCharge(mediator, additionalCharge);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetDefaultAdditionalChargeAmount(UKCompetentAuthority competentAuthority)
        {
            var response = new Core.SystemSetting.SystemSettingData();
            if (competentAuthority == UKCompetentAuthority.England)
            {
                response = await mediator.SendAsync(new GetSystemSettingById(SystemSettingType.EaAdditionalChargeFixedFee)); //EA
            }
            else if (competentAuthority == UKCompetentAuthority.Scotland)
            {
                response = await mediator.SendAsync(new GetSystemSettingById(SystemSettingType.SepaAdditionalChargeFixedFee)); //SEPA
            }

            return Json(response.Value);
        }

        private static Contact GetNewContactData(EditContactViewModel model)
        {
            var newContactData = new Contact
            {
                Email = model.Email,
                Name = model.FullName,
                TelephonePrefix = model.TelephonePrefix,
                Telephone = model.Telephone
            };
            return newContactData;
        }

        private static Address SetAddressData(EditContactViewModel model, Address oldAddress)
        {
            var newAddress = new Address
            {
                PostalCode = model.PostalCode,
                AddressLine1 = oldAddress.AddressLine1,
                AddressLine2 = oldAddress.AddressLine2,
                Country = oldAddress.Country,
                TownOrCity = oldAddress.TownOrCity
            };

            return newAddress;
        }
    }
}