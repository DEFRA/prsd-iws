namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using Core.ImportNotification.Summary;
    using EA.Iws.Requests.ImportNotification.Facilities;
    using EA.Iws.Requests.ImportNotification.Producers;
    using Infrastructure;
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

        public EditContactController(IMediator mediator, IAuditService auditService)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Exporter(Guid id)
        {
            var exporter = await mediator.SendAsync(new GetExporterByImportNotificationId(id));
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

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> Importer(Guid id)
        {
            var importer = await mediator.SendAsync(new GetImporterByImportNotificationId(id));
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

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> Producer(Guid id)
        {
            var producer = await mediator.SendAsync(new GetProducerByImportNotificationId(id));
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

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> Facility(Guid id)
        {
            var facility = await mediator.SendAsync(new GetFacilityByImportNotificationId(id));
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

            return RedirectToAction("Index", "Home");
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