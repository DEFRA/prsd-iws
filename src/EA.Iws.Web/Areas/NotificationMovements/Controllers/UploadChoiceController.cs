namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.NotificationMovements;
    using ViewModels.UploadChoice;

    [AuthorizeActivity(typeof(GetNewMovementIdsByNotificationId))]
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
        public async Task<ActionResult> Index(Guid notificationId)
        {
            var result = await mediator.SendAsync(new GetNewMovementIdsByNotificationId(notificationId));

            return View(new UploadChoiceViewModel(notificationId, result));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Guid notificationId, UploadChoiceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("Index", "Submit", new { id = model.RadioButtons.SelectedValue, area = "ExportMovement" });
        }
    }
}