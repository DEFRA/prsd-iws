namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.Notification;
    using ViewModels;

    public class OperationCompleteController : Controller
    {
        private readonly IMediator mediator;

        public OperationCompleteController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var notificationId = await mediator.SendAsync(new GetNotificationIdByMovementId(id));
            var notification = await mediator.SendAsync(new GetNotificationBasicInfo(notificationId));

            var model = new OperationCompleteViewModel
            {
                NotificationId = notificationId,
                NotificationType = notification.NotificationType
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Guid id, OperationCompleteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var fileExtension = Path.GetExtension(model.File.FileName);
            var uploadedFile = new byte[model.File.InputStream.Length];
            model.File.InputStream.Read(uploadedFile, 0, uploadedFile.Length);

            //TODO: await mediator

            return RedirectToAction("ApprovedNotification", "Applicant", new { id = model.NotificationId, area = string.Empty });
        }
    }
}