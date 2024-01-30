namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using Core.Notification.Audit;
    using EA.Iws.Requests.ImportNotification;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Requests.IntendedShipments;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.Shipment;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class ShipmentController : Controller
    {
        private readonly IMediator mediator;
        private readonly IAuditService auditService;

        public ShipmentController(IMediator mediator, IAuditService auditService)
        {
            this.mediator = mediator;
            this.auditService = auditService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var shipmentData = await mediator.SendAsync(new GetIntendedShipmentInfoForNotification(id));
            var model = new ShipmentInfoViewModel(shipmentData);

            var notificationInfo = await mediator.SendAsync(new GetNotificationDetails(id));
            if (notificationInfo.CompetentAuthority == Core.Notification.UKCompetentAuthority.Scotland)
            {
                model.ShowSelfEnterShipmentData = false;
            }

            // TODO - AAA Handle Self Entering Data Question show/hide database logic

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, ShipmentInfoViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingShipmentData = await mediator.SendAsync(new GetIntendedShipmentInfoForNotification(id));

            await mediator.SendAsync(model.ToRequest(id));

            await this.auditService.AddAuditEntry(this.mediator,
                   id,
                   User.GetUserId(),
                   existingShipmentData.HasShipmentData ? NotificationAuditType.Updated : NotificationAuditType.Added,
                   NotificationAuditScreenType.AmountsAndDates);

            if (backToOverview.GetValueOrDefault())
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "ChemicalComposition");
        }
    }
}