namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Exporters;
    using Core.Shared;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Exporters;
    using Requests.Importer;
    using Requests.NotificationAssessment;
    using ViewModels.EditContact;

    [AuthorizeActivity(typeof(SetExporterContact))]
    [AuthorizeActivity(typeof(SetImporterContact))]
    public class EditContactController : Controller
    {
        private readonly IMediator mediator;

        public EditContactController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Exporter(Guid id)
        {
            var exporter = await mediator.SendAsync(new GetExporterByNotificationId(id));
            var model = new EditContactViewModel(exporter);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Exporter(Guid id, EditContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var exporter = await mediator.SendAsync(new GetExporterByNotificationId(id));

            var contactData = GetNewContactData(model, exporter.Contact);

            await mediator.SendAsync(new SetExporterContact(id, contactData));

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
        public async Task<ActionResult> Importer(Guid id, EditContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var importer = await mediator.SendAsync(new GetImporterByNotificationId(id));

            var contactData = GetNewContactData(model, importer.Contact);

            await mediator.SendAsync(new SetImporterContact(id, contactData));

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
    }
}