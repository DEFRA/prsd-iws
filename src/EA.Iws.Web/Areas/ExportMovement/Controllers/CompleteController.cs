namespace EA.Iws.Web.Areas.ExportMovement.Controllers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.Movement.Complete;
    using Requests.Notification;
    using ViewModels.Complete;

    [AuthorizeActivity(typeof(SaveMovementCompletedReceipt))]
    public class CompleteController : Controller
    {
        private readonly IMediator mediator;
        private readonly IFileReader fileReader;
        private const string DateKey = "Date";

        public CompleteController(IMediator mediator, IFileReader fileReader)
        {
            this.mediator = mediator;
            this.fileReader = fileReader;
        }

        [HttpGet]
        public async Task<ActionResult> Date(Guid id)
        {
            var data = await mediator.SendAsync(new GetOperationCompleteData(id));

            var model = new DateViewModel(data);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Date(Guid id, DateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData[DateKey] = model.GetDateComplete();

            return RedirectToAction("Upload", "Complete");
        }

        [HttpGet]
        public async Task<ActionResult> Upload(Guid id)
        {
            object value;
            if (TempData.TryGetValue(DateKey, out value))
            {
                var completedDate = (DateTime)value;
                var notificationId = await mediator.SendAsync(new GetNotificationIdByMovementId(id));
                var notification = await mediator.SendAsync(new GetNotificationBasicInfo(notificationId));

                var model = new UploadViewModel
                {
                    NotificationId = notificationId,
                    NotificationType = notification.NotificationType,
                    CompletedDate = completedDate
                };

                return View(model);
            }

            return RedirectToAction("Date", "Complete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload(Guid id, UploadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var fileExtension = Path.GetExtension(model.File.FileName);
            var uploadedFile = await fileReader.GetFileBytes(model.File);

            await mediator.SendAsync(new SaveMovementCompletedReceipt(id, model.CompletedDate, uploadedFile, fileExtension));

            return RedirectToAction("Success", "Complete");
        }

        [HttpGet]
        public async Task<ActionResult> Success(Guid id)
        {
            var notificationId = await mediator.SendAsync(new GetNotificationIdByMovementId(id));
            var notification = await mediator.SendAsync(new GetNotificationBasicInfo(notificationId));
            var model = new SuccessViewModel
            {
                NotificationId = notificationId,
                NotificationType = notification.NotificationType
            };

            return View(model);
        }
    }
}