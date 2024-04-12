namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using Core.Notification.Audit;
    using EA.Iws.Core.SystemSettings;
    using EA.Iws.Requests.Notification;
    using EA.Iws.Requests.SystemSettings;
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

            model.ShowSelfEnterShipmentData = false;
            
            if (shipmentData.ShouldDisplayShipmentSelfEnterDataQuestion)
            {
                model.ShowSelfEnterShipmentData = true;

                var sepaFeeForNotSelfEnteringData = 
                    await mediator.SendAsync(new GetSystemSettingById(SystemSettingType.SepaFeeForNotSelfEnteringData));
                model.WillSelfEnterShipmentDataHintWithPrice =
                    "Please note if you select ‘No’ you will be charged £" + sepaFeeForNotSelfEnteringData.Value + 
                        " per shipment for SEPA staff to upload the shipment data on your behalf.";
            }

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