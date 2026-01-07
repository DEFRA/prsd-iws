namespace EA.Iws.Web.Controllers
{
    using EA.Iws.Requests.DeleteNotification;
    using EA.Iws.Requests.Notification;
    using EA.Iws.Web.Areas.Admin.Controllers;
    using EA.Iws.Web.Infrastructure.Authorization;
    using EA.Iws.Web.ViewModels.DeleteExportNotification;
    using EA.Prsd.Core.Mediator;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    [AuthorizeActivity(typeof(DeleteExportNotification))]
    public class DeleteExportNotificationController : Controller
    {
        private readonly IMediator mediator;

        public DeleteExportNotificationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new IndexViewModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var notificationDetails = await mediator.SendAsync(new GetExportNotificationId(model.NotificationNumber));

            if (notificationDetails != null && notificationDetails.IsNotificationCanDeleted == false)
            {
                if (string.IsNullOrEmpty(notificationDetails.ErrorMessage))
                {
                    ModelState.AddModelError("NotificationNumber", DeleteNotificationControllerResources.NumberNotExist);
                }
                else
                {
                    ModelState.AddModelError("NotificationNumber", notificationDetails.ErrorMessage);
                }

                    return View(model);
            }

            var deleteModel = new DeleteViewModel(model, notificationDetails.NotificationId);

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
            result = await mediator.SendAsync(new DeleteExportNotification(model.NotificationId.GetValueOrDefault()));
            model.Success = result;

            return View("Confirm", model);
        }
    }
}