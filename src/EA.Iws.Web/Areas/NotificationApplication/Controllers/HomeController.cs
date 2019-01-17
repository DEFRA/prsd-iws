namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Notification;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Notification;
    using Requests.SharedUsers;
    using Requests.Users;
    using ViewModels.Home;
    using ViewModels.NotificationApplication;

    [Authorize]
    public class HomeController : Controller
    {
        private readonly IMediator mediator;

        public HomeController(IMediator mediator)
        {
            this.mediator = mediator;
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
            var response = await mediator.SendAsync(new GetNotificationOverview(id));

            var model = new NotificationOverviewViewModel(response);

            model.SubmitSideBarViewModel.IsOwner = await mediator.SendAsync(new CheckIfNotificationOwner(id));

            if (!model.SubmitSideBarViewModel.IsOwner)
            {
                var sharedUsers = await mediator.SendAsync(new GetSharedUsersByNotificationId(id));
                model.SubmitSideBarViewModel.IsSharedUser = sharedUsers.Count(p => p.UserId == User.GetUserId()) > 0;
            }

            model.SubmitSideBarViewModel.IsInternalUser = await mediator.SendAsync(new GetUserIsInternal());

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
        public async Task<ActionResult> Resubmit(Guid id)
        {
            await mediator.SendAsync(new ResubmitNotification(id));

            if (User.IsInternalUser())
            {
                return RedirectToAction("Index", "KeyDates", new { area = "AdminExportAssessment", id });
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
    }
}