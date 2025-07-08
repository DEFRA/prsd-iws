namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Authorization.Permissions;
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.Notification.AdditionalCharge;
    using EA.Iws.Core.SystemSettings;
    using EA.Iws.Requests.AdditionalCharge;
    using EA.Iws.Requests.SystemSettings;
    using EA.Iws.Web.Infrastructure.AdditionalCharge;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using ViewModels.NumberOfShipments;

    [AuthorizeActivity(ImportNotificationPermissions.CanChangeNumberOfShipmentsOnImportNotification)]
    public class NumberOfShipmentsController : Controller
    {
        private readonly IMediator mediator;
        private readonly IAdditionalChargeService additionalChargeService;

        public NumberOfShipmentsController(IMediator mediator, IAdditionalChargeService additionalChargeService)
        {
            this.mediator = mediator;
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

            var importNotificationDetails = await mediator.SendAsync(new GetNotificationDetails(id));

            var confirmModel = new ConfirmViewModel(data, importNotificationDetails.CompetentAuthority, importNotificationDetails.Status);

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

            if (model.AdditionalCharge != null)
            {
                if (model.AdditionalCharge.IsAdditionalChargesRequired.HasValue && model.AdditionalCharge.IsAdditionalChargesRequired.Value)
                {
                    var addtionalCharge = new CreateImportNotificationAdditionalCharge(
                        model.NotificationId,
                        model.AdditionalCharge,
                        AdditionalChargeType.UpdateNumberOfShipment);

                    await additionalChargeService.AddImportAdditionalCharge(mediator, addtionalCharge);
                }
            }

            return RedirectToAction("Index", "Home");
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