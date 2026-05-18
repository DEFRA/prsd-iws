namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using Core.Notification.Audit;
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.NotificationAssessment;
    using EA.Iws.Core.SystemSettings;
    using EA.Iws.Core.WasteType;
    using EA.Iws.Requests.NotificationAssessment;
    using EA.Iws.Requests.SystemSettings;
    using EA.Iws.Requests.WasteType;
    using EA.Prsd.Core.Helpers;
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

                var sepaFeeForNotSelfEnteringData = await mediator.SendAsync(new GetSystemSettings(UKCompetentAuthority.Scotland, SystemSettingType.SepaFeeForNotSelfEnteringData));

                model.WillSelfEnterShipmentDataHintWithPrice = "Please note if you select ‘No’ you will be charged £" + sepaFeeForNotSelfEnteringData.Value +
                                                               " per shipment for SEPA staff to upload the shipment data on your behalf.";
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, ShipmentInfoViewModel model, bool? backToOverview = null)
        {
            WasteTypeData wasteTypeData = null;

            try
            {
                wasteTypeData = await mediator.SendAsync(new GetWasteType(id));
            }
            catch (Exception) 
            {
                // This is a test to make sure that if the waste type data cannot be retrieved, the user can still save the shipment data.
                // The waste type data is only used to validate the total shipments field, so if it cannot be retrieved, we will not perform that validation.
            }

            if (int.TryParse(model.NumberOfShipments, out int numberOfShipments) && wasteTypeData != null)
            {
                if ((numberOfShipments > 1) &&
                    (wasteTypeData.WasteCategoryType == WasteCategoryType.Singleship || 
                     wasteTypeData.WasteCategoryType == WasteCategoryType.Platformrig))
                {
                    ModelState.AddModelError("NumberOfShipments", "Only one shipment is allowed for Waste Category Type: " + EnumHelper.GetDisplayName<WasteCategoryType>((WasteCategoryType)wasteTypeData.WasteCategoryType));
                }
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingShipmentData = await mediator.SendAsync(new GetIntendedShipmentInfoForNotification(id));

            await mediator.SendAsync(model.ToRequest(id));

            if (existingShipmentData.HasShipmentData && existingShipmentData.NumberOfShipments != Convert.ToDecimal(model.NumberOfShipments)
                                                     && (existingShipmentData.Status == NotificationStatus.Transmitted ||
                                                         existingShipmentData.Status == NotificationStatus.Unlocked ||
                                                         existingShipmentData.Status == NotificationStatus.Consented ||
                                                         existingShipmentData.Status == NotificationStatus.ConsentedUnlock))
            {
                await mediator.SendAsync(new CreateNotificationStatusChange(id));
            }

            await auditService.AddAuditEntry(mediator, id, User.GetUserId(),
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