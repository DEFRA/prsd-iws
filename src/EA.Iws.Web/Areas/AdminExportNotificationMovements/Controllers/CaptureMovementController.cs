namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.Controllers
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
        public ActionResult Index(Guid id)
        {
            return View(new SearchViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, SearchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var movementId =
                await mediator.SendAsync(new GetMovementIdIfExists(id, model.Number.Value));

            if (!movementId.HasValue)
            {
                TempData[MovementNumberKey] = model.Number;
                return RedirectToAction("Create");
            }

            return RedirectToAction("Index", "InternalCapture", new { area = "AdminMovement", id = movementId.Value });
        }

        [HttpGet]
        public ActionResult Create(Guid id)
        {
            object result;
            if (TempData.TryGetValue(MovementNumberKey, out result))
            {
                return View(new CreateViewModel
                {
                    Number = (int)result,
                    NotificationId = id
                });
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Guid id, CreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = await mediator.SendAsync(new CreateMovementInternal(id, 
                model.Number,
                model.PrenotificationDate.AsDateTime(), 
                model.ActualShipmentDate.AsDateTime().Value));

            if (success)
            {
                var movementId = await mediator.SendAsync(new GetMovementIdByNumber(id, model.Number));

                return RedirectToAction("Index", "InternalCapture", new { area = "AdminMovement", id = movementId });
            }

            ModelState.AddModelError("Number", CaptureMovementControllerResources.SaveUnsuccessful);

            return View(model);
        }
    }
}