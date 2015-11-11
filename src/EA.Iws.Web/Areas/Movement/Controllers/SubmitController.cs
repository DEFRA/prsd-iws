namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using ViewModels;
    using ViewModels.Submit;

    [Authorize]
    public class SubmitController : Controller
    {
        private readonly IMediator mediator;

        public SubmitController(IMediator mediator)
        {
            this.mediator = mediator;
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
            var uploadedFile = new byte[model.File.InputStream.Length];
            model.File.InputStream.Read(uploadedFile, 0, uploadedFile.Length);

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