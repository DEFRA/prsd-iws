namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Exporters;
    using Core.Importer;
    using Core.Notification.Audit;
    using Core.Shared;
    using EA.Iws.Core.Facilities;
    using EA.Iws.Core.Producers;
    using EA.Iws.Requests.Facilities;
    using EA.Iws.Requests.Producers;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Exporters;
    using Requests.Importer;
    using Requests.NotificationAssessment;
    using ViewModels.EditContact;

    [AuthorizeActivity(typeof(SetExporterDetails))]
    [AuthorizeActivity(typeof(SetImporterDetails))]
    public class EditContactController : Controller
    {
        private readonly IMediator mediator;
        private readonly IAuditService auditService;

        public EditContactController(IMediator mediator, IAuditService auditService)
        {
            this.mediator = mediator;
            this.auditService = auditService;
        }

        [HttpGet]
        public async Task<ActionResult> Exporter(Guid id)
        {
            var exporter = await mediator.SendAsync(new GetExporterByNotificationId(id));
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

            var exporter = await mediator.SendAsync(new GetExporterByNotificationId(id));
            var exporterData = GetNewExporterData(model, exporter);
            await mediator.SendAsync(new SetExporterDetails(id, exporterData));

            await this.auditService.AddAuditEntry(this.mediator,
                    id,
                    User.GetUserId(),
                    NotificationAuditType.Updated,
                    NotificationAuditScreenType.Exporter);

            return RedirectToAction("Index", "Overview");
        }

        [HttpGet]
        public async Task<ActionResult> Importer(Guid id)
        {
            var importer = await mediator.SendAsync(new GetImporterByNotificationId(id));
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

            var importer = await mediator.SendAsync(new GetImporterByNotificationId(id));
            var importerData = GetNewImporterData(model, importer);
            await mediator.SendAsync(new SetImporterDetails(id, importerData));

            await this.auditService.AddAuditEntry(this.mediator,
                    id,
                    User.GetUserId(),
                    NotificationAuditType.Updated,
                    NotificationAuditScreenType.Importer);

            return RedirectToAction("Index", "Overview");
        }

        [HttpGet]
        public async Task<ActionResult> Producer(Guid id)
        {
            var producerList = await mediator.SendAsync(new GetProducersByNotificationId(id));
            var producer = producerList.FirstOrDefault();
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

            var producerList = await mediator.SendAsync(new GetProducersByNotificationId(id));
            var producer = producerList.FirstOrDefault();
            var producerData = GetNewProducerData(model, producer);
            await mediator.SendAsync(new SetProducerDetails(id, producerData));

            await this.auditService.AddAuditEntry(this.mediator,
                    id,
                    User.GetUserId(),
                    NotificationAuditType.Updated,
                    NotificationAuditScreenType.Producer);

            return RedirectToAction("Index", "Overview");
        }

        [HttpGet]
        public async Task<ActionResult> Facility(Guid id)
        {
            var facility = await mediator.SendAsync(new GetFacilitiesByNotificationId(id));
            var model = new EditContactViewModel(facility.FirstOrDefault());

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

            var facitiyList = await mediator.SendAsync(new GetFacilitiesByNotificationId(id));
            var facility = facitiyList.FirstOrDefault();
            var facilityData = GetNewFacilityData(model, facility);
            await mediator.SendAsync(new SetFacilityDetails(id, facilityData));

            await this.auditService.AddAuditEntry(this.mediator,
                    id,
                    User.GetUserId(),
                    NotificationAuditType.Updated,
                    NotificationAuditScreenType.DisposalFacilities);

            return RedirectToAction("Index", "Overview");
        }

        private static ContactData GetNewContactData(EditContactViewModel model, ContactData oldContactData)
        {
            var newContactData = new ContactData
            {
                Email = model.Email,
                FullName = model.FullName,
                TelephonePrefix = model.TelephonePrefix,
                Telephone = model.Telephone,
                FaxPrefix = oldContactData.FaxPrefix,
                Fax = oldContactData.Fax
            };
            return newContactData;
        }

        private static BusinessInfoData GetNewBusinessInfoData(EditContactViewModel model, BusinessInfoData oldBusinessInfoData)
        {
            var newBusinessInfoData = new BusinessInfoData
            {
                Name = model.Name,
                AdditionalRegistrationNumber = oldBusinessInfoData.AdditionalRegistrationNumber,
                BusinessType = oldBusinessInfoData.BusinessType,
                OtherDescription = oldBusinessInfoData.OtherDescription,
                RegistrationNumber = oldBusinessInfoData.RegistrationNumber
            };
            return newBusinessInfoData;
        }

        private static ExporterData GetNewExporterData(EditContactViewModel model, ExporterData oldExporterData)
        {
            var newExporterData = new ExporterData
            {
                Contact = GetNewContactData(model, oldExporterData.Contact),
                Business = GetNewBusinessInfoData(model, oldExporterData.Business)
            };
            return newExporterData;
        }

        private static ImporterData GetNewImporterData(EditContactViewModel model, ImporterData oldImporterData)
        {
            var newImporterData = new ImporterData
            {
                Contact = GetNewContactData(model, oldImporterData.Contact),
                Business = GetNewBusinessInfoData(model, oldImporterData.Business)
            };
            return newImporterData;
        }

        private static ProducerData GetNewProducerData(EditContactViewModel model, ProducerData oldProducerData)
        {
            var newProducerData = new ProducerData
            {
                Contact = GetNewContactData(model, oldProducerData.Contact),
                Business = GetNewBusinessInfoData(model, oldProducerData.Business)
            };

            return newProducerData;
        }

        private static FacilityData GetNewFacilityData(EditContactViewModel model, FacilityData oldFacilityData)
        {
            var newFacilityData = new FacilityData
            {
                Contact = GetNewContactData(model, oldFacilityData.Contact),
                Business = GetNewBusinessInfoData(model, oldFacilityData.Business)
            };

            return newFacilityData;
        }
    }
}