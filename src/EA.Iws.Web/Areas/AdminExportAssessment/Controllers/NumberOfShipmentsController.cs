namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using EA.Iws.Core.Authorization.Permissions;
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.Notification.AdditionalCharge;
    using EA.Iws.Core.Notification.Audit;
    using EA.Iws.Core.SystemSettings;
    using EA.Iws.Core.WasteType;
    using EA.Iws.Requests.AdditionalCharge;
    using EA.Iws.Requests.Notification;
    using EA.Iws.Requests.NotificationAssessment;
    using EA.Iws.Requests.SystemSettings;
    using EA.Iws.Requests.WasteType;
    using EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.NumberOfShipments;
    using EA.Iws.Web.Infrastructure;
    using EA.Iws.Web.Infrastructure.AdditionalCharge;
    using EA.Iws.Web.Infrastructure.Authorization;
    using EA.Prsd.Core.Helpers;
    using EA.Prsd.Core.Mediator;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    [AuthorizeActivity(ExportNotificationPermissions.CanChangeNumberOfShipmentsOnExportNotification)]
    public class NumberOfShipmentsController : Controller
    {
        private readonly IMediator mediator;
        private readonly IAuditService auditService;
        private readonly IAdditionalChargeService additionalChargeService;

        public NumberOfShipmentsController(IMediator mediator, IAuditService auditService, IAdditionalChargeService additionalChargeService)
        {
            this.mediator = mediator;
            this.auditService = auditService;
            this.additionalChargeService = additionalChargeService;
        }

        [HttpGet]
        public ActionResult Index(Guid id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, IndexViewModel model)
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

            if (model.Number != null && wasteTypeData != null)
            {
                if ((model.Number > 1) &&
                    (wasteTypeData.WasteCategoryType == WasteCategoryType.Singleship || 
                     wasteTypeData.WasteCategoryType == WasteCategoryType.Platformrig))
                {
                    ModelState.AddModelError("Number", "Only one shipment is allowed for Waste Category Type: " + EnumHelper.GetDisplayName<WasteCategoryType>((WasteCategoryType)wasteTypeData.WasteCategoryType));
                }
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("Confirm", model);
        }

        [HttpGet]
        public async Task<ActionResult> Confirm(Guid id, IndexViewModel model)
        {
            var data = await mediator.SendAsync(new GetChangeNumberOfShipmentConfrimationData(id, model.Number.GetValueOrDefault()));
            var competentAuthority = (await mediator.SendAsync(new GetNotificationBasicInfo(id))).CompetentAuthority;
            var notificationStatus = await mediator.SendAsync(new GetNotificationStatus(id));
            var confirmModel = new ConfirmViewModel(data, competentAuthority, notificationStatus);
            confirmModel.NewNumberOfShipments = model.Number.GetValueOrDefault();

            return View(confirmModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Confirm(ConfirmViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.SendAsync(new SetNewNumberOfShipments(model.NotificationId, model.OldNumberOfShipments, model.NewNumberOfShipments));

            await this.auditService.AddAuditEntry(this.mediator,
                    model.NotificationId,
                    User.GetUserId(),
                    NotificationAuditType.Updated,
                    NotificationAuditScreenType.AmountsAndDates);

            if (model.AdditionalCharge != null)
            {
                if (model.AdditionalCharge.IsAdditionalChargesRequired.HasValue && model.AdditionalCharge.IsAdditionalChargesRequired.Value)
                {
                    var addtionalCharge = new CreateAdditionalCharge(model.NotificationId, model.AdditionalCharge, AdditionalChargeType.UpdateNumberOfShipment);

                    await additionalChargeService.AddAdditionalCharge(mediator, addtionalCharge);
                }
            }

            return RedirectToAction("Index", "Overview");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetDefaultAdditionalChargeAmount(UKCompetentAuthority competentAuthority)
        {
            var response = new Core.SystemSetting.SystemSettingData();
            if (competentAuthority == UKCompetentAuthority.England)
            {
                response = await mediator.SendAsync(new GetSystemSettings(competentAuthority, SystemSettingType.EaAdditionalChargeFixedFee));
            }
            else if (competentAuthority == UKCompetentAuthority.Scotland)
            {
                response = await mediator.SendAsync(new GetSystemSettings(competentAuthority, SystemSettingType.SepaAdditionalChargeFixedFee));
            }

            return Json(response.Value);
        }
    }
}