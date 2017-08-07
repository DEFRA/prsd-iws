namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using ViewModels.Success;
    using ViewModels.Upload;

    [AuthorizeActivity(typeof(GetMovementsByIds))]
    public class UploadController : Controller
    {
        private readonly IFileReader fileReader;
        private readonly IMediator mediator;

        public UploadController(IMediator mediator, IFileReader fileReader)
        {
            this.mediator = mediator;
            this.fileReader = fileReader;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid notificationId, Guid[] movementIds)
        {
            ViewBag.NotificationId = notificationId;
            var movements = await mediator.SendAsync(new GetMovementsByIds(notificationId, movementIds));
            var model = new UploadViewModel(movements);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid notificationId, Guid[] movementIds, UploadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.NotificationId = notificationId;
                var movements = await mediator.SendAsync(new GetMovementsByIds(notificationId, movementIds));
                model = new UploadViewModel(movements);
                return View(model);
            }

            var fileExtension = Path.GetExtension(model.File.FileName);
            var uploadedFile = await fileReader.GetFileBytes(model.File);

            await mediator.SendAsync(new SetMultipleMovementFileId(notificationId, movementIds, uploadedFile, fileExtension));

            return RedirectToAction("Success", movementIds.ToRouteValueDictionary("movementIds"));
        }

        [HttpGet]
        public async Task<ActionResult> Success(Guid notificationId, Guid[] movementIds)
        {
            ViewBag.NotificationId = notificationId;
            var movements = await mediator.SendAsync(new GetMovementsByIds(notificationId, movementIds));
            var model = new SuccessViewModel(movements);
            return View(model);
        }
    }
}