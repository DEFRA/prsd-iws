namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using Core.FinancialGuarantee;
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.Notification.AdditionalCharge;
    using EA.Iws.Core.Shared;
    using EA.Iws.Core.SystemSettings;
    using EA.Iws.Requests.AdditionalCharge;
    using EA.Iws.Requests.Notification;
    using EA.Iws.Requests.SystemSettings;
    using EA.Iws.Web.Infrastructure.AdditionalCharge;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.FinancialGuaranteeAssessment;

    [AuthorizeActivity(typeof(GetFinancialGuaranteeDataByNotificationApplicationId))]
    public class FinancialGuaranteeAssessmentController : Controller
    {
        private readonly IMediator mediator;
        private readonly IAdditionalChargeService additionalChargeService;

        public FinancialGuaranteeAssessmentController(IMediator mediator, IAdditionalChargeService additionalChargeService)
        {
            this.mediator = mediator;
            this.additionalChargeService = additionalChargeService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var currentFinancialGuarantee = await mediator.SendAsync(new GetCurrentFinancialGuaranteeDetails(id));

            return View(currentFinancialGuarantee);
        }

        [HttpGet]
        public async Task<ActionResult> New(Guid id)
        {
            var currentFinancialGuarantee = await mediator.SendAsync(new GetCurrentFinancialGuaranteeDetails(id));
            var model = new NewFinancialGuaranteeViewModel();
            if (currentFinancialGuarantee != null && currentFinancialGuarantee.Status == 0)
            {
                model = new NewFinancialGuaranteeViewModel()
                {
                    HasAlreadyFinancialGuarantee = false
                };
            }
            else
            {
                var competentAuthority = (await mediator.SendAsync(new GetNotificationBasicInfo(id))).CompetentAuthority;
                model = new NewFinancialGuaranteeViewModel()
                {
                    HasAlreadyFinancialGuarantee = (currentFinancialGuarantee.Status == FinancialGuaranteeStatus.Approved) ? true : false,
                    AdditionalCharge = new AdditionalChargeData()
                    {
                        NotificationId = id
                    },
                    CompetentAuthority = competentAuthority
                };
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> New(Guid id, NewFinancialGuaranteeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.SendAsync(new CreateFinancialGuarantee(id, model.ReceivedDate.AsDateTime().Value));

            if (model.AdditionalCharge != null)
            {
                if (model.AdditionalCharge.IsAdditionalChargesRequired.HasValue && model.AdditionalCharge.IsAdditionalChargesRequired.Value)
                {
                    var addtionalCharge = CreateAdditionalChargeData(model.AdditionalCharge.NotificationId, model.AdditionalCharge, AdditionalChargeType.FinancialGuaranteeAssessment);

                    await additionalChargeService.AddAdditionalCharge(mediator, addtionalCharge);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Complete(Guid id, Guid financialGuaranteeId)
        {
            var financialGuarantee = await mediator.SendAsync(
                new GetFinancialGuaranteeDataByNotificationApplicationId(id, financialGuaranteeId));

            if (financialGuarantee.Status != FinancialGuaranteeStatus.ApplicationReceived)
            {
                return RedirectToAction("Index");
            }

            var model = new CompleteFinancialGuaranteeViewModel
            {
                FinancialGuaranteeId = financialGuaranteeId,
                ReceivedDate = financialGuarantee.ReceivedDate
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Complete(Guid id, CompleteFinancialGuaranteeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.SendAsync(
                new CompleteFinancialGuarantee(id, model.FinancialGuaranteeId, model.CompleteDate.AsDateTime().Value));

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetDefaultAdditionalChargeAmount(UKCompetentAuthority competentAuthority)
        {
            var response = new Core.SystemSetting.SystemSettingData();
            if (competentAuthority == UKCompetentAuthority.England)
            {
                response = await mediator.SendAsync(new GetSystemSettingById(SystemSettingType.EaAdditionalChargeFixedFee)); //EA
            }
            else if (competentAuthority == UKCompetentAuthority.Scotland)
            {
                response = await mediator.SendAsync(new GetSystemSettingById(SystemSettingType.SepaAdditionalChargeFixedFee)); //SEPA
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