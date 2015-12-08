namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.NotificationMovements;
    using ViewModels.UploadChoice;

    [Authorize]
    public class UploadChoiceController : Controller
    {
        private static readonly Dictionary<MovementNumberStatus, string> ValidationMessages = new Dictionary<MovementNumberStatus, string>
        {
            { MovementNumberStatus.DoesNotExist, UploadChoiceControllerResources.DoesNotExist },
            { MovementNumberStatus.NotNew, UploadChoiceControllerResources.NotNew },
            { MovementNumberStatus.OutOfRange, UploadChoiceControllerResources.OutOfRange }
        };

        private readonly IMediator mediator;

        public UploadChoiceController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new UploadChoiceViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid notificationId, UploadChoiceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var status = await mediator.SendAsync(new CheckMovementNumberValid(notificationId, model.Number.Value));

            if (status == MovementNumberStatus.Valid)
            {
                var movementId = await mediator.SendAsync(new GetMovementIdByNumber(notificationId, model.Number.Value));

                return RedirectToAction("Index", "Submit", new { id = movementId, area = "ExportMovement" });
            }

            ModelState.AddModelError("Number", ValidationMessages[status]);

            return View(model);
        }
    }
}