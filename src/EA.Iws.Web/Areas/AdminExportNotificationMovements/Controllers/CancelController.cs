﻿namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Movement;
    using Core.Shared;
    using Infrastructure.Authorization;
    using Prsd.Core.Helpers;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using ViewModels.Cancel;
    using Resources = CancelControllerResources;

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
            var submittedMovements = await mediator.SendAsync(new GetSubmittedPendingMovements(id));

            var addedMovements = GetTempDataAddedCancellableMovements().Where(x => x.NotificationId == id).ToList();
            TempData[AddedCancellableMovementsListKey] = addedMovements;

            var model = new SelectMovementsViewModel(submittedMovements, addedMovements);

            var selectedMovements = GetTempDataSelectedMovements().Where(x => x.NotificationId == id).ToList();
            TempData[SubmittedMovementListKey] = selectedMovements;
            if (selectedMovements.Count > 0)
            {
                var selectedMovementIds = selectedMovements.Select(m => m.Id).ToArray();

                foreach (var movement in model.SubmittedMovements.Where(x => selectedMovementIds.Contains(x.MovementId)))
                {
                    movement.IsSelected = true;
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, SelectMovementsViewModel model, string command)
        {
            var selectedMovements = model.SubmittedMovements
                .Where(m => m.IsSelected)
                .Select(p => new MovementData { NotificationId = id, Id = p.MovementId, Number = p.Number })
                .ToList();

            TempData[SubmittedMovementListKey] = selectedMovements;

            if (command == AddCommand)
            {
                return RedirectToAction("Add");
            }

            var addedCancellableMovements = GetTempDataAddedCancellableMovements();

            int removeShipmentNumber;
            if (!string.IsNullOrEmpty(command) && int.TryParse(command, out removeShipmentNumber))
            {
                addedCancellableMovements.RemoveAll(x => x.Number == removeShipmentNumber);
                TempData[AddedCancellableMovementsListKey] = addedCancellableMovements;
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid && !addedCancellableMovements.Any())
            {
                model.SubmittedMovements = await mediator.SendAsync(new GetSubmittedPendingMovements(id));
                model.AddedMovements = addedCancellableMovements;
                return View(model);
            }

            return RedirectToAction("Confirm");
        }
        
        [HttpGet]
        public ActionResult Add(Guid id)
        {
            var model = new AddViewModel(GetTempDataAddedCancellableMovements());

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
                    ModelState.AddModelError(Resources.ShipmentNumberField,
                        string.Format(Resources.ExceedShpmentLimit, AddedCancellableMovementsLimit));
                }
                if (addedCancellableMovements.Any(x => x.Number == model.ShipmentNumber))
                {
                    ModelState.AddModelError(Resources.ShipmentNumberField, string.Format(Resources.DuplicateShipmentNumber, model.ShipmentNumber));
                }

                var shipmentValidationResult =
                    await mediator.SendAsync(new IsAddedCancellableMovementValid(id, model.ShipmentNumber));

                if (shipmentValidationResult.IsCancellableExistingShipment)
                {
                    ModelState.AddModelError(Resources.ShipmentNumberField, string.Format(Resources.IsCancellableExistingShipment, model.ShipmentNumber));
                }
                if (shipmentValidationResult.IsNonCancellableExistingShipment)
                {
                    var completedDisplay = shipmentValidationResult.NotificationType == NotificationType.Recovery
                        ? Resources.Recovered
                        : Resources.Disposed;

                    ModelState.AddModelError(Resources.ShipmentNumberField,
                        string.Format(Resources.IsNonCancellableExistingShipment,
                            shipmentValidationResult.Status == MovementStatus.Completed
                                ? completedDisplay
                                : EnumHelper.GetDisplayName(shipmentValidationResult.Status), model.ShipmentNumber));
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
            var selectedMovements = GetTempDataSelectedMovements();
            var addedMovements = GetTempDataAddedCancellableMovements();

            if (!selectedMovements.Any() && !addedMovements.Any())
            {
                return RedirectToAction("Index");
            }

            var model = new ConfirmViewModel(id, selectedMovements, addedMovements);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Confirm(Guid id, ConfirmViewModel model)
        {
            var selectedMovements = GetTempDataSelectedMovements();
            var addedMovements = GetTempDataAddedCancellableMovements();

            if (!selectedMovements.Any() && !addedMovements.Any())
            {
                return RedirectToAction("Index");
            }

            await mediator.SendAsync(new CancelMovements(model.NotificationId, selectedMovements, addedMovements));

            return RedirectToAction("Success");
        }

        [HttpGet]
        public ActionResult Success(Guid id)
        {
            var selectedMovements = GetTempDataSelectedMovements();
            var addedMovements = GetTempDataAddedCancellableMovements();

            if (!selectedMovements.Any() && !addedMovements.Any())
            {
                return RedirectToAction("Index");
            }

            var shipmentNumbers = selectedMovements.Select(m => m.Number).ToList();
            shipmentNumbers.AddRange(addedMovements.Select(x => x.Number));

            return View(new SuccessViewModel(id, shipmentNumbers));
        }

        [HttpGet]
        public ActionResult AddAbandon(Guid id)
        {
            TempData[AddedCancellableMovementsListKey] = null;

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Abandon(Guid id)
        {
            TempData[SubmittedMovementListKey] = null;
            TempData[AddedCancellableMovementsListKey] = null;

            return RedirectToAction("Index", "Home");
        }

        private List<MovementData> GetTempDataSelectedMovements()
        {
            var result = new List<MovementData>();

            object selectedMovements;
            if (TempData.TryGetValue(SubmittedMovementListKey, out selectedMovements))
            {
                result = selectedMovements as List<MovementData> ?? result;

                TempData[SubmittedMovementListKey] = result;
            }

            return result;
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

            return result.OrderBy(x => x.Number).ToList();
        }
    }
}