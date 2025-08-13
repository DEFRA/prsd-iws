namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using Core.Authorization.Permissions;
    using Core.Notification.Audit;
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.Notification.AdditionalCharge;
    using EA.Iws.Core.Shared;
    using EA.Iws.Requests.AdditionalCharge;
    using EA.Iws.Requests.Notification;
    using EA.Iws.Requests.SystemSettings;
    using EA.Iws.Web.Infrastructure.AdditionalCharge;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
    using ViewModels.NumberOfShipments;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using EA.Iws.Core.SystemSettings;

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
        public ActionResult Index(Guid id, IndexViewModel model)
        {
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