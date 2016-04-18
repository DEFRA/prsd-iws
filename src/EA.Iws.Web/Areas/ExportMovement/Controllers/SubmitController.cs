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
    using ViewModels;
    using ViewModels.Submit;

    [AuthorizeActivity(typeof(SetMovementFileId))]
    public class SubmitController : Controller
    {
        private readonly IMediator mediator;
        private readonly IFileReader fileReader;

        public SubmitController(IMediator mediator, IFileReader fileReader)
        {
            this.mediator = mediator;
            this.fileReader = fileReader;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var notificationId = await mediator.SendAsync(new GetNotificationIdByMovementId(id));

            var model = new SubmitViewModel
            {
                NotificationId = notificationId,
                MovementId = id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, SubmitViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var fileExtension = Path.GetExtension(model.File.FileName);
            var uploadedFile = await fileReader.GetFileBytes(model.File);

            await mediator.SendAsync(new SetMovementFileId(id, uploadedFile, fileExtension));

            return RedirectToAction("Success", "Submit", new { id = model.MovementId});
        }

        [HttpGet]
        public async Task<ActionResult> Success(Guid id)
        {
            var notificationId = await mediator.SendAsync(new GetNotificationIdByMovementId(id));
            var movementNumber = await mediator.SendAsync(new GetMovementNumberByMovementId(id));

            var model = new SuccessViewModel
            {
                MovementNumber = movementNumber,
                NotificationId = notificationId
            };

            return View(model);
        }
    }
}