namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.NotificationMovements.Capture;
    using ViewModels.CaptureMovement;

    [Authorize(Roles = "internal")]
    public class CaptureMovementController : Controller
    {
        private const string MovementNumberKey = "MovementNumberKey";
        private readonly IMediator mediator;

        public CaptureMovementController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid notificationId)
        {
            var nextNumber = await mediator.SendAsync(new GetNextAvailableMovementNumberForNotification(notificationId));

            return View(new SearchViewModel
            {
                Number = nextNumber
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid notificationId, SearchViewModel model)
        {
            var movementId =
                await mediator.SendAsync(new GetMovementIdIfExists(notificationId, model.Number));

            if (!movementId.HasValue)
            {
                TempData[MovementNumberKey] = model.Number;
                return RedirectToAction("Create");
            }

            return RedirectToAction("Index", "InternalCapture", new { area = "Movement", id = movementId.Value });
        }

        [HttpGet]
        public ActionResult Create(Guid notificationId)
        {
            object result;
            if (TempData.TryGetValue(MovementNumberKey, out result))
            {
                return View(new CreateViewModel
                {
                    Number = (int)result
                });
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Guid notificationId, CreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = await mediator.SendAsync(new CreateMovementInternal(notificationId, 
                model.Number,
                model.PrenotificationDate.AsDateTime(), 
                model.ActualShipmentDate.AsDateTime().Value));

            if (success)
            {
                var movementId = await mediator.SendAsync(new GetMovementIdByNumber(notificationId, model.Number));

                return RedirectToAction("Index", "InternalCapture", new { area = "Movement", id = movementId });
            }

            ModelState.AddModelError("Number", CaptureMovementControllerResources.SaveUnsuccessful);

            return View(model);
        }
    }
}