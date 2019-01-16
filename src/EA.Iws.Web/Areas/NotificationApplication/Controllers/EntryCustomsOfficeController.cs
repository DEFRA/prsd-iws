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
        public async Task<ActionResult> Index(Guid id)
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
                    Steps = (data.CustomsOfficesRequired == CustomsOffices.EntryAndExit) ? 2 : 1
                };
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, CustomsOfficeViewModel model)
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

            await mediator.SendAsync(new SetEntryCustomsOfficeForNotificationById(id,
                model.Name,
                model.Address,
                model.SelectedCountry.Value));

            await this.auditService.AddAuditEntry(this.mediator,
                   id,
                   User.GetUserId(),
                   existingData.CustomsOfficeData == null ? NotificationAuditType.Create : NotificationAuditType.Update,
                   "Customs office");

            return RedirectToAction("Index", "Shipment", new { id });
        }
    }
}