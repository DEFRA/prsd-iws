namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Movement.Receive;
    using ViewModels.Reject;

    [AuthorizeActivity(typeof(GetSubmittedMovementsByNotificationId))]
    public class RejectController : Controller
    {
        private readonly IMediator mediator;

        public RejectController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid notificationId)
        {
            var result = await mediator.SendAsync(new GetSubmittedMovementsByNotificationId(notificationId));

            return View(new RejectViewModel
            {
                Movements = result,
                NotificationId = notificationId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(RejectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("Index", "Reject", new { area = "ExportMovement", id = model.Selected.Value });
        }
    }
}