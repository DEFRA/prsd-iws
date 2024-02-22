namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using Core.Notification.Audit;
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.Notification.AdditionalCharge;
    using EA.Iws.Core.Shared;
    using EA.Iws.Core.SystemSetting;
    using EA.Iws.Core.SystemSettings;
    using EA.Iws.Requests.AdditionalCharge;
    using EA.Iws.Requests.Notification;
    using EA.Iws.Requests.SystemSettings;
    using EA.Iws.Web.Infrastructure.AdditionalCharge;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.NotificationAssessment;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.Carrier;

    [AuthorizeActivity(typeof(AddCarrier))]
    public class CarrierController : Controller
    {
        private readonly IMediator mediator;
        private readonly IAuditService auditService;
        private readonly IAdditionalChargeService additionalChargeService;

        public CarrierController(IMediator mediator, IAuditService auditService, IAdditionalChargeService additionalChargeService)
        {
            this.mediator = mediator;
            this.auditService = auditService;
            this.additionalChargeService = additionalChargeService;
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id)
        {
            var competentAuthority = (await mediator.SendAsync(new GetNotificationBasicInfo(id))).CompetentAuthority;
            var notificationStatus = await mediator.SendAsync(new GetNotificationStatus(id));
            var model = new AddCarrierViewModel(id, competentAuthority, notificationStatus);

            await this.BindCountryList(mediator);
            model.Address.DefaultCountryId = this.GetDefaultCountryId();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(AddCarrierViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(mediator);
                return View(model);
            }
            try
            {
                await mediator.SendAsync(model.ToRequest());

                await auditService.AddAuditEntry(mediator,
                    model.NotificationId,
                    User.GetUserId(),
                    NotificationAuditType.Added,
                    NotificationAuditScreenType.IntendedCarrier);

                if (model.AdditionalCharge != null)
                {
                    if (model.AdditionalCharge.IsAdditionalChargesRequired.HasValue && model.AdditionalCharge.IsAdditionalChargesRequired.Value)
                    {
                        var addtionalCharge = CreateAdditionalChargeData(model.NotificationId, model.AdditionalCharge, AdditionalChargeType.AddCarrier);

                        await additionalChargeService.AddAdditionalCharge(mediator, addtionalCharge);
                    }
                }

                return RedirectToAction("Index", "Overview");
            }
            catch (ApiBadRequestException ex)
            {
                this.HandleBadRequest(ex);
                if (ModelState.IsValid)
                {
                    throw;
                }
            }

            await this.BindCountryList(mediator);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetDefaultAdditionalChargeAmount(UKCompetentAuthority competentAuthority)
        {
            var response = new SystemSettingData();
            if (competentAuthority == UKCompetentAuthority.England)
            {
                response = await mediator.SendAsync(new GetSystemSettingById(SystemSettingType.EaAdditionalChargeFixedFee)); //Id = 5 = EA
            }
            else if (competentAuthority == UKCompetentAuthority.Scotland)
            {
                response = await mediator.SendAsync(new GetSystemSettingById(SystemSettingType.SepaAdditionalChargeFixedFee)); //Id = 5 = SEPA
            }

            return Json(response.Value);
        }

        private static CreateAdditionalCharge CreateAdditionalChargeData(Guid notificationId, AdditionalChargeData model, AdditionalChargeType additionalChargeType)
        {
            var createAddtionalCharge = new CreateAdditionalCharge()
            {
                ChangeDetailType = additionalChargeType,
                ChargeAmount = model.Amount,
                Comments = model.Comments,
                NotificationId = notificationId
            };

            return createAddtionalCharge;
        }
    }
}