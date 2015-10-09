namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using ViewModels.CancelMovement;

    [Authorize]
    public class CancelMovementController : Controller
    {
        private readonly IMediator mediator;

        public CancelMovementController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid notificationId)
        {
            var result = await mediator.SendAsync(new GetSubmittedMovements(notificationId));

            var model = new CancelMovementsViewModel(result);
            model.NotificationId = notificationId;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(CancelMovementsViewModel model)
        {
            //TODO: Replace with next screen navigation
            return await Index(model.NotificationId);
        }
    }
}