namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using Requests.ImportNotificationAssessment;
    using Requests.Notification;
    using Requests.NotificationAssessment;
    using ViewModels.DeleteNotification;

    [AuthorizeActivity(typeof(DeleteExportNotification))]
    public class DeleteNotificationController : Controller
    {
        private readonly IMediator mediator;

        public DeleteNotificationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Guid? notificationId;

            if (model.IsExportNotification.GetValueOrDefault())
            {
                notificationId = await mediator.SendAsync(new GetNotificationIdByNumber(model.NotificationNumber));
            }
            else
            {
                notificationId = await mediator.SendAsync(new GetImportNotificationIdByNumber(model.NotificationNumber));
            }

            if (notificationId == null)
            {
                ModelState.AddModelError("NotificationNumber", DeleteNotificationControllerResources.NumberNotExist);

                return View(model);
            }

            var deleteModel = new DeleteViewModel(model, notificationId.GetValueOrDefault());

            return RedirectToAction("Check", deleteModel);
        }

        [HttpGet]
        public ActionResult Check(DeleteViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(DeleteViewModel model)
        {
            bool result = false;

            if (model.IsExportNotification)
            {
                result = await mediator.SendAsync(new DeleteExportNotification(model.NotificationId.GetValueOrDefault()));
            }

            if (!model.IsExportNotification)
            {
                result = await mediator.SendAsync(new DeleteImportNotification(model.NotificationId.GetValueOrDefault()));
            }

            model.Success = result;

            return View("Confirm", model);
        }
    }
}