namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.Movement.Receive;
    using ViewModels.Cancel;

    [Authorize(Roles = "internal")]
    public class CancelController : Controller
    {
        private readonly IMediator mediator;
        private const string SubmittedMovementListKey = "SubmittedMovementListKey";

        public CancelController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var result = await mediator.SendAsync(new GetSubmittedMovements(id));

            var model = new SelectMovementsViewModel
            {
                SubmittedMovements = result
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, SelectMovementsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.SubmittedMovements = await mediator.SendAsync(new GetSubmittedMovements(id));
                return View(model);
            }

            var selectedMovements = model.SubmittedMovements
               .Where(m => m.IsSelected)
               .Select(p => new MovementData { Id = p.MovementId, Number = p.Number })
               .ToList();

            TempData[SubmittedMovementListKey] = selectedMovements;

            return RedirectToAction("Confirm");
        }

        [HttpGet]
        public ActionResult Confirm(Guid id)
        {
            object result;
            if (TempData.TryGetValue(SubmittedMovementListKey, out result))
            {
                var selectedMovements = result as List<MovementData>;

                var model = new ConfirmViewModel
                {
                    NotificationId = id,
                    SelectedMovements = selectedMovements
                };

                TempData[SubmittedMovementListKey] = selectedMovements;

                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Confirm(Guid id, ConfirmViewModel model)
        {
            object result;
            if (TempData.TryGetValue(SubmittedMovementListKey, out result))
            {
                var selectedMovements = result as List<MovementData>;

                TempData[SubmittedMovementListKey] = selectedMovements;

                await mediator.SendAsync(new CancelMovements(model.NotificationId, selectedMovements));

                return RedirectToAction("Success");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Success(Guid id)
        {
            object result;
            if (TempData.TryGetValue(SubmittedMovementListKey, out result))
            {
                var selectedMovements = result as List<MovementData>;

                var shipmentNumbers = selectedMovements.Select(m => m.Number).ToList();

                return View(new SuccessViewModel(id, shipmentNumbers));
            }

            return RedirectToAction("Index");
        }
    }
}