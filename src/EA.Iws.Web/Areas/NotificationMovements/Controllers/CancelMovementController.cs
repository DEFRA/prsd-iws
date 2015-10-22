namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using ViewModels.CancelMovement;

    [Authorize]
    public class CancelMovementController : Controller
    {
        private readonly IMediator mediator;
        private const string SubmittedMovementListKey = "SubmittedMovementList";

        public CancelMovementController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid notificationId)
        {
            var result = await mediator.SendAsync(new GetSubmittedMovements(notificationId));

            var model = new CancelMovementsViewModel(notificationId, result);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(CancelMovementsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return await Index(model.NotificationId);
            }

            var selectedMovements = model.SubmittedMovements
                .Where(m => m.IsSelected)
                .Select(p => new MovementData { Id = p.MovementId, Number = p.Number })
                .ToList();

            TempData[SubmittedMovementListKey] = selectedMovements;

            return RedirectToAction("Confirm", "CancelMovement", new { notificationId = model.NotificationId });
        }

        [HttpGet]
        public ActionResult Confirm(Guid notificationId)
        {
            object result;
            if (TempData.TryGetValue(SubmittedMovementListKey, out result))
            {
                var selectedMovements = result as List<MovementData>;

                var model = new ConfirmCancelMovementsViewModel(notificationId, selectedMovements);

                TempData[SubmittedMovementListKey] = selectedMovements;

                return View(model);
            }

            return RedirectToAction("Index", "CancelMovement", new { notificationId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Confirm(ConfirmCancelMovementsViewModel model)
        {
            object result;
            if (TempData.TryGetValue(SubmittedMovementListKey, out result))
            {
                var selectedMovements = result as List<MovementData>;

                TempData[SubmittedMovementListKey] = selectedMovements;

                await mediator.SendAsync(new CancelMovements(model.NotificationId, selectedMovements));

                return RedirectToAction("Success", "CancelMovement", new { notificationId = model.NotificationId });
            }

            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult Success(Guid notificationId)
        {
            object result;
            if (TempData.TryGetValue(SubmittedMovementListKey, out result))
            {
                var selectedMovements = result as List<MovementData>;

                var shipmentNumbers = selectedMovements.Select(m => m.Number).ToList();

                return View(new SuccessViewModel(notificationId, shipmentNumbers));
            }

            return RedirectToAction("Index", "CancelMovement", new { notificationId });
        }
    }
}