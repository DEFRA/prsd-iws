namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Movement;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Requests.NotificationAssessment;
    using Requests.NotificationMovements;
    using ViewModels.Options;

    [Authorize]
    public class OptionsController : Controller
    {
        private readonly IMediator mediator;

        public OptionsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, int? status)
        {
            var movementsSummary = await mediator.SendAsync(new GetSummaryAndTable(id, (MovementStatus?)status));
            var notificationBasicInformation = await mediator.SendAsync(new GetNotificationBasicInfo(id));
            var notificationStatus = await mediator.SendAsync(new GetNotificationStatus(id));

            var model = new ShipmentSummaryViewModel(id, movementsSummary);
            model.SelectedMovementStatus = (MovementStatus?)status;
            model.CompetentAuthority = notificationBasicInformation.CompetentAuthority;
            model.NotificationStatus = notificationStatus;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public ActionResult IndexPost(Guid id, int? selectedMovementStatus)
        {
            return RedirectToAction("Index", new { id, status = selectedMovementStatus });
        }

        [HttpGet]
        public ActionResult Unavailable(Guid id)
        {
            var model = new UnavailableViewModel { NotificationId = id };

            return View(model);
        }
    }
}