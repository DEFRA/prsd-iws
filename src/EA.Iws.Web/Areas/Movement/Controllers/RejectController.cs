namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.Movement.Reject;
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
        public ActionResult Index(Guid id)
        {
            return View(new RejectViewModel());
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

            var notificationId = await mediator.SendAsync(new GetNotificationIdByMovementId(id));

            return RedirectToAction("Index", "Home", new { area = "NotificationMovements", notificationId });
        }
    }
}