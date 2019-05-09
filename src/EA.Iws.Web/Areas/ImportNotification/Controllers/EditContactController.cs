namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.ImportNotification.Summary;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification.Exporters;
    using Requests.ImportNotification.Importers;
    using ViewModels.EditContact;

    [AuthorizeActivity(typeof(SetExporterContactForImportNotification))]
    [AuthorizeActivity(typeof(SetImporterContactForImportNotification))]
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

            var contactData = GetNewContactData(model, exporter.Contact);

            await mediator.SendAsync(new SetExporterContactForImportNotification(id, contactData));

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

            var exporter = await mediator.SendAsync(new GetImporterByImportNotificationId(id));

            var contactData = GetNewContactData(model, exporter.Contact);

            await mediator.SendAsync(new SetImporterContactForImportNotification(id, contactData));

            return RedirectToAction("Index", "Home");
        }

        private static Contact GetNewContactData(EditContactViewModel model, Contact oldContactData)
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
    }
}