namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using ViewModels.Movement;

    [Authorize]
    public class NotificationMovementController : Controller
    {
        private readonly IMediator mediator;

        public NotificationMovementController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var model = new MovementsViewModel
            {
                NotificationId = id,
                Movements = await mediator.SendAsync(new GetMovementsForNotificationById(id))
            };

            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Guid id)
        {
            var movementId = await mediator.SendAsync(new CreateMovementForNotificationById(id));

            return RedirectToAction("Index", "ShipmentDate", new { id = movementId, area = "Movement" });
        }
    }
}