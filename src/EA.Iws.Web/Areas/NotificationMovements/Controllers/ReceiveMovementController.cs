namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Movement.Receive;
    using ViewModels.ReceiveMovement;

    [AuthorizeActivity(typeof(GetSubmittedMovementsByNotificationId))]
    public class ReceiveMovementController : Controller
    {
        private readonly IMediator mediator;

        public ReceiveMovementController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid notificationId)
        {
            var result = await mediator.SendAsync(new GetSubmittedMovementsByNotificationId(notificationId));

            return View(new MovementReceiptViewModel(notificationId, result));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Guid notificationId, MovementReceiptViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("Index", "DateReceived",
                new
                {
                    id = model.RadioButtons.SelectedValue,
                    area = "ExportMovement"
                });
        }
    }
}