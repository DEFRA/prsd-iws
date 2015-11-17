namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using NotificationMovements.ViewModels.Edit;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    [Authorize]
    public class EditDateController : Controller
    {
        private readonly IMediator mediator;

        public EditDateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Index(Guid id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, EditViewModel model)
        {
            var notificationId = await mediator.SendAsync(new GetNotificationIdByMovementId(id));

            return RedirectToAction("Index", "Home", new { area = "NotificationMovements", notificationId });
        }
    }
}