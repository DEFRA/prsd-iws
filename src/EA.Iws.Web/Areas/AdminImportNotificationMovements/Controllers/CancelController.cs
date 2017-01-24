namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.ImportMovement;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement.Cancel;
    using Requests.ImportNotificationMovements;
    using ViewModels.Cancel;

    [AuthorizeActivity(typeof(CancelImportMovements))]
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
            var result = await mediator.SendAsync(new GetCancellableMovements(id));

            var model = new CancellableMovementsViewModel(result);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, CancellableMovementsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var result = await mediator.SendAsync(new GetCancellableMovements(id));
                model = new CancellableMovementsViewModel(result);

                return View(model);
            }

            var selectedMovements = model.CancellableMovements
               .Where(m => m.IsSelected)
               .Select(p => new ImportCancelMovementData { Id = p.MovementId, Number = p.Number })
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
                var selectedMovements = result as List<ImportCancelMovementData>;

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
                var selectedMovements = result as List<ImportCancelMovementData>;

                TempData[SubmittedMovementListKey] = selectedMovements;

                await mediator.SendAsync(new CancelImportMovements(selectedMovements));

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
                var selectedMovements = result as List<ImportCancelMovementData>;

                var shipmentNumbers = selectedMovements.Select(m => m.Number).ToList();

                return View(new SuccessViewModel(id, shipmentNumbers));
            }

            return RedirectToAction("Index");
        }
    }
}