namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Movement;
    using Infrastructure.Authorization;
    using Prsd.Core.Helpers;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using ViewModels.Cancel;

    [AuthorizeActivity(typeof(CancelMovements))]
    [AuthorizeActivity(typeof(IsAddedCancellableMovementValid))]
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
        public async Task<ActionResult> Add(Guid id, AddViewModel model, string command)
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
                TempData[AddedCancellableMovementsListKey] = addedCancellableMovements;

                model.AddedMovements = addedCancellableMovements;

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (addedCancellableMovements.Any(x => x.Number == model.ShipmentNumber))
                {
                    ModelState.AddModelError("NewShipmentNumber",
                        "This Shipment number already exists in the table below and will be added to the list of shipments that will be cancelled.");
                }

                var shipmentValidationResult =
                    await mediator.SendAsync(new IsAddedCancellableMovementValid(id, model.ShipmentNumber));

                if (shipmentValidationResult.IsCancellableExistingShipment)
                {
                    ModelState.AddModelError("NewShipmentNumber",
                        "This Shipment number already exists and is shown on the previous screen. Please tick the shipment number on that screen.");
                }
                if (shipmentValidationResult.IsNonCancellableExistingShipment)
                {
                    ModelState.AddModelError("NewShipmentNumber",
                        string.Format(
                            "This Shipment number already exists but the status is {0}. Seek further advice of how to proceed with the data team leader.",
                            EnumHelper.GetDisplayName(shipmentValidationResult.Status)));
                }

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                addedCancellableMovements.Add(new AddedCancellableMovement()
                {
                    NotificationId = id,
                    Number = model.ShipmentNumber,
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