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
        private const int AddedCancellableMovementsLimit = 20;

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
                SubmittedMovements = result,
                AddedMovements = GetTempDataAddedCancellableMovements()
            };

            object cancellableMovements;
            if (TempData.TryGetValue(SubmittedMovementListKey, out cancellableMovements))
            {
                var selectedMovements = cancellableMovements as List<MovementData>;

                if (selectedMovements.Count > 0)
                {
                    var selectedMovementIds = selectedMovements.Select(m => m.Id).ToArray();

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
        public async Task<ActionResult> Index(Guid id, SelectMovementsViewModel model, string command)
        {
            var addedCancellableMovements = GetTempDataAddedCancellableMovements();

            int removeShipmentNumber;
            if (!string.IsNullOrEmpty(command) && int.TryParse(command, out removeShipmentNumber))
            {
                addedCancellableMovements.RemoveAll(x => x.Number == removeShipmentNumber);
                TempData[AddedCancellableMovementsListKey] = addedCancellableMovements;
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                model.SubmittedMovements = await mediator.SendAsync(new GetSubmittedPendingMovements(id));
                model.AddedMovements = addedCancellableMovements;
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
            var model = new AddViewModel()
            {
                AddedMovements = GetTempDataAddedCancellableMovements()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(Guid id, AddViewModel model, string command)
        {
            var addedCancellableMovements = GetTempDataAddedCancellableMovements();

            if (command == AddCommand)
            {
                model.AddedMovements = addedCancellableMovements;

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (addedCancellableMovements.Count >= AddedCancellableMovementsLimit)
                {
                    ModelState.AddModelError("NewShipmentNumber",
                        string.Format(
                            "You cannot add more than {0} extra records at a time. If more are needed to be added, please carry out this process a further time after confirmation as taken place.",
                            AddedCancellableMovementsLimit));
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

        private List<AddedCancellableMovement> GetTempDataAddedCancellableMovements()
        {
            var result = new List<AddedCancellableMovement>();

            object addedMovements;
            if (TempData.TryGetValue(AddedCancellableMovementsListKey, out addedMovements))
            {
                result = addedMovements as List<AddedCancellableMovement> ?? result;

                TempData[AddedCancellableMovementsListKey] = result;
            }

            return result;
        }
    }
}