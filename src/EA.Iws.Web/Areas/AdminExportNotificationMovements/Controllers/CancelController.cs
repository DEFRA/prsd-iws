namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Movement;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using ViewModels.Cancel;

    [AuthorizeActivity(typeof(CancelMovements))]
    public class CancelController : Controller
    {
        private readonly IMediator mediator;
        private const string SubmittedMovementListKey = "SubmittedMovementListKey";
        private const string AddedCancellableMovementsListKey = "AddedCancellableMovementsListKey";
        private const string AddCommand = "add";

        public CancelController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var result = await mediator.SendAsync(new GetSubmittedPendingMovements(id));

            var model = new SelectMovementsViewModel
            {
                SubmittedMovements = result
            };

            object cancellableMovements;
            if (TempData.TryGetValue(SubmittedMovementListKey, out cancellableMovements))
            {
                var selectedMovements = cancellableMovements as List<MovementData>;

                if (selectedMovements.Count > 0)
                {
                    Guid[] selectedMovementIds = selectedMovements.Select(m => m.Id).ToArray();

                    foreach (var movement in model.SubmittedMovements.Where(x => selectedMovementIds.Contains(x.MovementId)))
                    {
                        movement.IsSelected = true;
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, SelectMovementsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.SubmittedMovements = await mediator.SendAsync(new GetSubmittedPendingMovements(id));
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
        public ActionResult Add(Guid id)
        {
            var model = new AddViewModel();

            object result;
            if (TempData.TryGetValue(AddedCancellableMovementsListKey, out result))
            {
                var addedCancellableMovements = result as List<AddedCancellableMovement>;

                TempData[AddedCancellableMovementsListKey] = addedCancellableMovements;

                model.AddedMovements = addedCancellableMovements;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Guid id, AddViewModel model, string command)
        {
            var addedCancellableMovements = new List<AddedCancellableMovement>();
            object result;
            if (TempData.TryGetValue(AddedCancellableMovementsListKey, out result))
            {
                addedCancellableMovements = result as List<AddedCancellableMovement> ??
                                            addedCancellableMovements;
            }

            if (command == AddCommand)
            {
                if (!ModelState.IsValid)
                {
                    TempData[AddedCancellableMovementsListKey] = addedCancellableMovements;

                    model.AddedMovements = addedCancellableMovements;

                    return View(model);
                }

                addedCancellableMovements.Add(new AddedCancellableMovement()
                {
                    NotificationId = id,
                    Number = model.NewShipmentNumber.Value,
                    ShipmentDate = model.NewActualShipmentDate.Value
                });
            }

            int removeShipmentNumber;
            if (int.TryParse(command, out removeShipmentNumber))
            {
                addedCancellableMovements.RemoveAll(x => x.Number == removeShipmentNumber);
            }

            TempData[AddedCancellableMovementsListKey] = addedCancellableMovements;

            return RedirectToAction("Add");
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