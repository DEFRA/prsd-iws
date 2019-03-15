namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using Core.CustomsOffice;
    using Core.Notification.Audit;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Requests.CustomsOffice;
    using Requests.Shared;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.CustomsOffice;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class EntryCustomsOfficeController : Controller
    {
        private readonly IMediator mediator;
        private readonly IAuditService auditService;

        public EntryCustomsOfficeController(IMediator mediator, IAuditService auditService)
        {
            this.mediator = mediator;
            this.auditService = auditService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, bool? backToOverview = null)
        {
            var data = await mediator.SendAsync(new GetEntryCustomsOfficeAddDataByNotificationId(id));

            if (data.CustomsOfficesRequired != CustomsOffices.EntryAndExit
                && data.CustomsOfficesRequired != CustomsOffices.Entry)
            {
                return RedirectToAction("Index", "CustomsOffice", new { id });
            }

            CustomsOfficeViewModel model;
            if (data.CustomsOfficeData != null)
            {
                model = new CustomsOfficeViewModel
                {
                    Address = data.CustomsOfficeData.Address,
                    Name = data.CustomsOfficeData.Name,
                    SelectedCountry = data.CustomsOfficeData.Country.Id,
                    Countries = new SelectList(data.Countries, "Id", "Name", data.CustomsOfficeData.Country.Id),
                    Steps = (data.CustomsOfficesRequired == CustomsOffices.EntryAndExit) ? 2 : 1
                };
            }
            else
            {
                model = new CustomsOfficeViewModel
                {
                    Countries = new SelectList(data.Countries, "Id", "Name"),
                    Steps = (data.CustomsOfficesRequired == CustomsOffices.EntryAndExit) ? 2 : 1,
                    CustomsOfficeRequired = true
                };
            }

            var existing = await mediator.SendAsync(new GetEntryExitCustomsOfficeSelectionForNotificationById(id));

            if (existing == null)
            {
                model.CustomsOfficeRequired = null;
            }
            else
            {
                model.CustomsOfficeRequired = existing.Entry;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, CustomsOfficeViewModel model, bool? backToOverview = null)
        {
            var countries = await mediator.SendAsync(new GetEuropeanUnionCountries());

            model.Countries = model.SelectedCountry.HasValue
                ? new SelectList(countries, "Id", "Name", model.SelectedCountry.Value)
                : new SelectList(countries, "Id", "Name");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingData = await mediator.SendAsync(new GetEntryCustomsOfficeAddDataByNotificationId(id));
            NotificationAuditType auditType = NotificationAuditType.Added;

            if (model.CustomsOfficeRequired.GetValueOrDefault())
            {
                await mediator.SendAsync(new SetEntryCustomsOfficeForNotificationById(id,
                    model.Name,
                    model.Address,
                    model.SelectedCountry.Value));
                auditType = existingData.CustomsOfficeData == null ? NotificationAuditType.Added : NotificationAuditType.Updated;
            }
            else if (existingData.CustomsOfficeData != null)
            {
                // If customs office required is set to false but there is existing data in the database, delete it
                await mediator.SendAsync(new DeleteEntryCustomsOfficeByNotificationId(id));
                auditType = NotificationAuditType.Deleted;
            }

            await this.auditService.AddAuditEntry(this.mediator,
                       id,
                       User.GetUserId(),
                       auditType,
                       NotificationAuditScreenType.CustomsOffice);

            await mediator.SendAsync(new SetEntryCustomsOfficeSelectionForNotificationById(id, model.CustomsOfficeRequired.GetValueOrDefault()));

            return RedirectToAction("Index", "ExitCustomsOffice", new { id, backToOverview = backToOverview });
        }
    }
}