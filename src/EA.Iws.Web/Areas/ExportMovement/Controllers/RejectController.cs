namespace EA.Iws.Web.Areas.ExportMovement.Controllers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.Movement.Reject;
    using ViewModels;
    using ViewModels.Reject;

    [Authorize]
    public class RejectController : Controller
    {
        private readonly IMediator mediator;

        public RejectController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var model = new RejectViewModel();
            var notificationId = await mediator.SendAsync(new GetNotificationIdByMovementId(id));
            model.NotificationId = notificationId;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, RejectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var fileExtension = Path.GetExtension(model.File.FileName);
            var uploadedFile = new byte[model.File.InputStream.Length];
            model.File.InputStream.Read(uploadedFile, 0, uploadedFile.Length);

            await mediator.SendAsync(new SetMovementRejected(id, 
                model.RejectionDate.AsDateTime().Value, 
                model.RejectionReason,
                uploadedFile,
                fileExtension));

            return RedirectToAction("Success");
        }

        [HttpGet]
        public async Task<ActionResult> Success(Guid id)
        {
            var notificationId = await mediator.SendAsync(new GetNotificationIdByMovementId(id));
            var movementNumber = await mediator.SendAsync(new GetMovementNumberByMovementId(id));

            var model = new SuccessViewModel
            {
                NotificationId = notificationId,
                MovementNumber = movementNumber
            };

            return View(model);
        }
    }
}