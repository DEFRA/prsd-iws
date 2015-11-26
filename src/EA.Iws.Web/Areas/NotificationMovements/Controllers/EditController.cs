namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.Movement.Receive;
    using ViewModels.Edit;

    [Authorize]
    public class EditController : Controller
    {
        private readonly IMediator mediator;

        public EditController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid notificationId)
        {
            var submittedMovements = await mediator.SendAsync(new GetSubmittedMovementsByNotificationId(notificationId));

            var model = new EditViewModel(submittedMovements);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Guid notificationId, EditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("Index", "EditDate", new { area = "Movement", id = model.Shipments.SelectedValue });
        }
    }
}