namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using Core.Notification;
    using EA.Iws.Core.Notification.AdditionalCharge;
    using EA.Iws.Core.NotificationAssessment;
    using EA.Iws.Core.Shared;
    using EA.Iws.Core.SystemSettings;
    using EA.Iws.Requests.AdditionalCharge;
    using EA.Iws.Requests.SystemSettings;
    using EA.Iws.Web.Infrastructure.AdditionalCharge;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Notification;
    using Requests.SharedUsers;
    using Requests.Users;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.Home;
    using ViewModels.NotificationApplication;

    [Authorize]
    public class HomeController : Controller
    {
        private readonly IMediator mediator;
        private readonly IAdditionalChargeService additionalChargeService;

        public HomeController(IMediator mediator, IAdditionalChargeService additionalChargeService)
        {
            this.mediator = mediator;
            this.additionalChargeService = additionalChargeService;
        }

        [HttpGet]
        public async Task<ActionResult> GenerateNotificationDocument(Guid id)
        {
            try
            {
                var response = await mediator.SendAsync(new GenerateNotificationDocument(id));

                return File(response.Content, MimeTypeHelper.GetMimeType(response.FileNameWithExtension),
                    response.FileNameWithExtension);
            }
            catch (ApiBadRequestException ex)
            {
                this.HandleBadRequest(ex);
                if (ModelState.IsValid)
                {
                    throw;
                }
                return HttpNotFound();
            }
        }

        [HttpGet]
        public async Task<ActionResult> GenerateNotificationPreviewDocument(Guid id)
        {
            try
            {
                var response = await mediator.SendAsync(new GenerateNotificationPreviewDocument(id));

                return File(response.Content, MimeTypeHelper.GetMimeType(response.FileNameWithExtension),
                    response.FileNameWithExtension);
            }
            catch (ApiBadRequestException ex)
            {
                this.HandleBadRequest(ex);
                if (ModelState.IsValid)
                {
                    throw;
                }
                return HttpNotFound();
            }
        }

        [HttpGet]
        public async Task<ActionResult> GenerateInterimMovementDocument(Guid id)
        {
            try
            {
                var response = await mediator.SendAsync(new GenerateInterimMovementDocument(id));

                return File(response.Content, MimeTypeHelper.GetMimeType(response.FileNameWithExtension),
                    response.FileNameWithExtension);
            }
            catch (ApiBadRequestException ex)
            {
                this.HandleBadRequest(ex);
                if (ModelState.IsValid)
                {
                    throw;
                }
                return HttpNotFound();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var isArchived = await mediator.SendAsync(new GetArchivedNotificationById(id));
            if (isArchived)
            {
                return RedirectToAction("NotFound", "Applicant", new { area = string.Empty });
            }

            var response = await mediator.SendAsync(new GetNotificationOverview(id));

            var model = new NotificationOverviewViewModel(response);

            model.SubmitSideBarViewModel.IsOwner = await mediator.SendAsync(new CheckIfNotificationOwner(id));

            if (!model.SubmitSideBarViewModel.IsOwner)
            {
                var sharedUsers = await mediator.SendAsync(new GetSharedUsersByNotificationId(id));
                model.SubmitSideBarViewModel.IsSharedUser = sharedUsers.Count(p => p.UserId == User.GetUserId()) > 0;
            }

            model.SubmitSideBarViewModel.IsInternalUser = await mediator.SendAsync(new GetUserIsInternal());

            //Here restricting the external user to edit the consented unlock notification
            if (!model.SubmitSideBarViewModel.IsInternalUser && model.SubmitSideBarViewModel.Status == NotificationStatus.ConsentedUnlock)
            {
                model.CanEditNotification = false;
                model.SubmitSideBarViewModel.ShowResubmitButton = false;
            }
            else if (model.SubmitSideBarViewModel.Status == NotificationStatus.Unlocked || model.SubmitSideBarViewModel.Status == NotificationStatus.ConsentedUnlock)
            {
                model.SubmitSideBarViewModel.ShowResubmitButton = true;
                model.AdditionalCharge = new AdditionalChargeData()
                {
                    NotificationId = id
                };
            }

            ViewBag.Charge = response.NotificationCharge;

            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult _Navigation(Guid id)
        {
            if (id == Guid.Empty)
            {
                return PartialView(new NotificationApplicationCompletionProgress());
            }

            var response = Task.Run(() => mediator.SendAsync(new GetNotificationProgressInfo(id))).Result;

            return PartialView(response);
        }        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Resubmit(Guid id, NotificationOverviewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var response = await mediator.SendAsync(new GetNotificationOverview(id));

                model = new NotificationOverviewViewModel(response);

                model.SubmitSideBarViewModel.IsOwner = await mediator.SendAsync(new CheckIfNotificationOwner(id));

                if (!model.SubmitSideBarViewModel.IsOwner)
                {
                    var sharedUsers = await mediator.SendAsync(new GetSharedUsersByNotificationId(id));
                    model.SubmitSideBarViewModel.IsSharedUser = sharedUsers.Count(p => p.UserId == User.GetUserId()) > 0;
                }

                model.SubmitSideBarViewModel.IsInternalUser = await mediator.SendAsync(new GetUserIsInternal());

                //Here restricting the external user to edit the consented unlock notification
                if (!model.SubmitSideBarViewModel.IsInternalUser && model.SubmitSideBarViewModel.Status == NotificationStatus.ConsentedUnlock)
                {
                    model.CanEditNotification = false;
                    model.SubmitSideBarViewModel.ShowResubmitButton = false;
                }
                else if (model.SubmitSideBarViewModel.Status == NotificationStatus.Unlocked || model.SubmitSideBarViewModel.Status == NotificationStatus.ConsentedUnlock)
                {
                    model.SubmitSideBarViewModel.ShowResubmitButton = true;
                    model.SubmitSideBarViewModel.AdditionalCharge = new AdditionalChargeData()
                    {
                        NotificationId = id
                    };
                }

                ViewBag.Charge = response.NotificationCharge;

                return View(model);
            }

            await mediator.SendAsync(new ResubmitNotification(model.NotificationId));

            if (User.IsInternalUser())
            {
                if (model.AdditionalCharge != null)
                {
                    if (model.AdditionalCharge.IsAdditionalChargesRequired.HasValue && model.AdditionalCharge.IsAdditionalChargesRequired.Value)
                    {
                        var addtionalCharge = CreateAdditionalChargeData(model.NotificationId, model.AdditionalCharge, AdditionalChargeType.EditExportDetails);

                        await additionalChargeService.AddAdditionalCharge(mediator, addtionalCharge);
                    }
                }

                return RedirectToAction("Index", "KeyDates", new { area = "AdminExportAssessment", model.NotificationId });
            }

            return RedirectToAction("ResubmissionSuccess");
        }

        [HttpGet]
        public async Task<ActionResult> ResubmissionSuccess(Guid id)
        {
            var details = await mediator.SendAsync(new GetNotificationBasicInfo(id));

            var model = new ResubmissionSuccessViewModel(details);

            return View(model);
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