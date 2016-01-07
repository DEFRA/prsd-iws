namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Movement;
    using Core.PackagingType;
    using Core.Rules;
    using Core.Shared;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.NotificationMovements;
    using Requests.NotificationMovements.Create;
    using ViewModels.Create;

    [Authorize]
    public class CreateController : Controller
    {
        private readonly IMediator mediator;
        private const string MovementNumberKey = "MovementNumberKey";
        private const string ShipmentDateKey = "ShipmentDateKey";
        private const string QuantityKey = "QuantityKey";
        private const string UnitKey = "UnitKey";
        private const string PackagingTypesKey = "PackagingTypesKey";
        private const string NumberOfCarriersKey = "NumberOfCarriersKey";
        private const string NumberOfPackagesKey = "NumberOfPackagesKey";

        public CreateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> ShipmentDate(Guid notificationId)
        {
            var ruleSummary = await mediator.SendAsync(new GetMovementRulesSummary(notificationId));

            if (!ruleSummary.IsSuccess)
            {
                return GetRuleErrorView(ruleSummary);
            }

            return await ReturnShipmentDateView(notificationId);
        }

        private async Task<ActionResult> ReturnShipmentDateView(Guid notificationId)
        {
            var movementNumber = await mediator.SendAsync(new GenerateMovementNumber(notificationId));
            var shipmentDates = await mediator.SendAsync(new GetShipmentDates(notificationId));

            ViewBag.MovementNumber = movementNumber;
            var model = new ShipmentDateViewModel(shipmentDates, movementNumber);

            return View("ShipmentDate", model);
        }

        private ActionResult GetRuleErrorView(MovementRulesSummary ruleSummary)
        {
            if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.TotalShipmentsReached && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("TotalMovementsReached");
            }
            else if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.TotalIntendedQuantityReached && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("TotalIntendedQuantityReached");
            }
            else if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.TotalIntendedQuantityExceeded && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("TotalIntendedQuantityExceeded");
            }
            else if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.ActiveLoadsReached && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("TotalActiveLoadsReached");
            }
            else if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.ConsentPeriodExpired && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("ConsentPeriodExpired");
            }
            else if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.ConsentExpiresInFourWorkingDays && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("ConsentExpiresInFourWorkingDays");
            }
            else if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.ConsentExpiresInThreeOrLessWorkingDays && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("ConsentExpiresInThreeOrLessWorkingDays");
            }
            else if (ruleSummary.RuleResults.Any(r => r.Rule == MovementRules.ConsentWithdrawn && r.MessageLevel == MessageLevel.Error))
            {
                return RedirectToAction("ConsentWithdrawn");
            }

            throw new InvalidOperationException("Unknown rule view");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ShipmentDate(Guid notificationId, ShipmentDateViewModel model)
        {
            TempData[MovementNumberKey] = model.MovementNumber;
            TempData[ShipmentDateKey] = model.AsDateTime();

            if (!ModelState.IsValid)
            {
                ViewBag.MovementNumber = model.MovementNumber;
                return View(model);
            }

            var workingDaysUntilShipment = await mediator.SendAsync(new GetWorkingDaysUntil(notificationId, model.AsDateTime().GetValueOrDefault()));

            if (workingDaysUntilShipment < 4)
            {
                return RedirectToAction("ThreeWorkingDaysWarning", "Create");
            }

            return RedirectToAction("Quantity", "Create");
        }

        [HttpGet]
        public ActionResult ThreeWorkingDaysWarning(Guid notificationId)
        {
            object result;
            if (TempData.TryGetValue(MovementNumberKey, out result))
            {
                var movementNumber = (int)result;

                ViewBag.MovementNumber = movementNumber;
                var model = new ThreeWorkingDaysWarningViewModel(movementNumber);

                return View("ThreeWorkingDays", model);
            }

            return RedirectToAction("ShipmentDate", "Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ThreeWorkingDaysWarning(Guid notificationId, ThreeWorkingDaysWarningViewModel model)
        {
            TempData[MovementNumberKey] = model.MovementNumber;

            if (!ModelState.IsValid)
            {
                ViewBag.MovementNumber = model.MovementNumber;
                return View("ThreeWorkingDays", model);
            }

            if (model.Selection == ThreeWorkingDaysSelection.ChangeDate)
            {
                TempData[ShipmentDateKey] = model.DateInput.AsDateTime();

                var workingDaysUntilShipment = await mediator.SendAsync(new GetWorkingDaysUntil(notificationId, model.DateInput.AsDateTime().GetValueOrDefault()));

                if (workingDaysUntilShipment < 4)
                {
                    return RedirectToAction("ThreeWorkingDaysWarning", "Create");
                }
            }

            return RedirectToAction("Quantity", "Create");
        }

        [HttpGet]
        public async Task<ActionResult> Quantity(Guid notificationId)
        {
            object result;
            if (TempData.TryGetValue(MovementNumberKey, out result))
            {
                var movementNumber = (int)result;
                var shipmentUnits = await mediator.SendAsync(new GetShipmentUnits(notificationId));

                ViewBag.MovementNumber = movementNumber;
                var model = new QuantityViewModel(shipmentUnits, movementNumber);

                return View(model);
            }

            return RedirectToAction("ShipmentDate", "Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Quantity(Guid notificationId, QuantityViewModel model)
        {
            TempData[MovementNumberKey] = model.MovementNumber;
            TempData[QuantityKey] = model.Quantity;
            TempData[UnitKey] = model.Units;

            var hasExceededTotalQuantity =
                await mediator.SendAsync(new HasExceededConsentedQuantity(notificationId, Convert.ToDecimal(model.Quantity), model.Units.Value));

            if (hasExceededTotalQuantity)
            {
                ModelState.AddModelError("Quantity", QuantityViewModelResources.HasExceededTotalQuantity);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.MovementNumber = model.MovementNumber;
                return View(model);
            }

            return RedirectToAction("PackagingTypes", "Create");
        }

        [HttpGet]
        public async Task<ActionResult> PackagingTypes(Guid notificationId)
        {
            object result;
            if (TempData.TryGetValue(MovementNumberKey, out result))
            {
                var movementNumber = (int)result;
                var availablePackagingTypes = await mediator.SendAsync(new GetPackagingTypes(notificationId));

                ViewBag.MovementNumber = movementNumber;
                var model = new PackagingTypesViewModel(availablePackagingTypes, movementNumber);

                return View(model);
            }

            return RedirectToAction("ShipmentDate", "Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PackagingTypes(Guid notificationId, PackagingTypesViewModel model)
        {
            TempData[MovementNumberKey] = model.MovementNumber;
            TempData[PackagingTypesKey] = model.SelectedValues;

            if (!ModelState.IsValid)
            {
                ViewBag.MovementNumber = model.MovementNumber;
                return View(model);
            }

            return RedirectToAction("NumberOfPackages", "Create");
        }

        [HttpGet]
        public ActionResult NumberOfPackages(Guid notificationId)
        {
            object result;
            if (TempData.TryGetValue(MovementNumberKey, out result))
            {
                var movementNumber = (int)result;

                ViewBag.MovementNumber = movementNumber;
                var model = new NumberOfPackagesViewModel(movementNumber);

                return View(model);
            }

            return RedirectToAction("ShipmentDate", "Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NumberOfPackages(Guid notificationId, NumberOfPackagesViewModel model)
        {
            TempData[MovementNumberKey] = model.MovementNumber;
            TempData[NumberOfPackagesKey] = model.Number;

            if (!ModelState.IsValid)
            {
                ViewBag.MovementNumber = model.MovementNumber;
                return View(model);
            }

            return RedirectToAction("NumberOfCarriers", "Create");
        }

        [HttpGet]
        public async Task<ActionResult> NumberOfCarriers(Guid notificationId)
        {
            object result;
            if (TempData.TryGetValue(MovementNumberKey, out result))
            {
                var movementsNumber = (int)result;
                var meansOfTransport = await mediator.SendAsync(new GetMeansOfTransport(notificationId));

                ViewBag.MovementNumber = movementsNumber;
                var meansOfTransportViewModel = new MeansOfTransportViewModel { NotificationMeansOfTransport = meansOfTransport };
                var model = new NumberOfCarriersViewModel(meansOfTransportViewModel, movementsNumber);

                return View(model);
            }

            return RedirectToAction("ShipmentDate", "Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NumberOfCarriers(Guid notificationId, NumberOfCarriersViewModel model)
        {
            TempData[MovementNumberKey] = model.MovementNumber;
            TempData[NumberOfCarriersKey] = model.Number;

            if (!ModelState.IsValid)
            {
                ViewBag.MovementNumber = model.MovementNumber;
                var meansOfTransport = await mediator.SendAsync(new GetMeansOfTransport(notificationId));
                model.MeansOfTransportViewModel = new MeansOfTransportViewModel { NotificationMeansOfTransport = meansOfTransport };

                return View(model);
            }

            return RedirectToAction("Carriers", "Create");
        }

        [HttpGet]
        public async Task<ActionResult> Carriers(Guid notificationId)
        {
            object movementNumberResult;
            object numberOfCarriersResult;
            if (TempData.TryGetValue(MovementNumberKey, out movementNumberResult)
                && TempData.TryGetValue(NumberOfCarriersKey, out numberOfCarriersResult))
            {
                var movementNumber = (int)movementNumberResult;
                var numberOfCarriers = (int)numberOfCarriersResult;
                var meansOfTransport = await mediator.SendAsync(new GetMeansOfTransport(notificationId));
                var notificationCarriers = await mediator.SendAsync(new GetCarriers(notificationId));

                ViewBag.MovementNumber = movementNumber;
                var meansOfTransportViewModel = new MeansOfTransportViewModel { NotificationMeansOfTransport = meansOfTransport };
                var model = new CarrierViewModel(notificationCarriers, numberOfCarriers, meansOfTransportViewModel, movementNumber);

                TempData[MovementNumberKey] = movementNumber;
                TempData[NumberOfCarriersKey] = numberOfCarriers;

                return View(model);
            }

            return RedirectToAction("ShipmentDate", "Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Carriers(Guid notificationId, CarrierViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.MovementNumber = model.MovementNumber;
                model.MeansOfTransportViewModel = new MeansOfTransportViewModel
                {
                    NotificationMeansOfTransport = await mediator.SendAsync(new GetMeansOfTransport(notificationId))
                };
                
                return View(model);
            }

            object shipmentDateResult;
            object quantityResult;
            object unitResult;
            object numberOfPackagesResult;
            object packagingTypesResult;

            var tempDataExists = TempData.TryGetValue(ShipmentDateKey, out shipmentDateResult);
            tempDataExists &= TempData.TryGetValue(QuantityKey, out quantityResult);
            tempDataExists &= TempData.TryGetValue(UnitKey, out unitResult);
            tempDataExists &= TempData.TryGetValue(NumberOfPackagesKey, out numberOfPackagesResult);
            tempDataExists &= TempData.TryGetValue(PackagingTypesKey, out packagingTypesResult);

            if (tempDataExists)
            {
                var shipmentDate = (DateTime)shipmentDateResult;
                var quantity = Convert.ToDecimal(quantityResult);
                var unit = (ShipmentQuantityUnits)unitResult;
                var numberOfPackages = (int)numberOfPackagesResult;
                var packagingTypes = (IList<PackagingType>)packagingTypesResult;

                var selectedCarriers = new Dictionary<int, Guid>();

                for (int i = 0; i < model.SelectedItems.Count; i++)
                {
                    selectedCarriers.Add(i, model.SelectedItems[i].Value);
                }

                var newMovementDetails = new NewMovementDetails
                {
                    Quantity = quantity,
                    Units = unit,
                    PackagingTypes = packagingTypes,
                    NumberOfPackages = numberOfPackages,
                    OrderedCarriers = selectedCarriers
                };

                var newMovementId = await mediator.SendAsync(new CreateMovementAndDetails(notificationId, shipmentDate, newMovementDetails));

                return RedirectToAction("Download", "Create", new { id = newMovementId });
            }

            return RedirectToAction("ShipmentDate", "Create");
        }

        [HttpGet]
        public async Task<ActionResult> Download(Guid notificationId, Guid id)
        {
            var model = new DownloadViewModel { MovementId = id };

            ViewBag.MovementNumber = await mediator.SendAsync(new GetMovementNumberByMovementId(id));

            return View(model);
        }

        [HttpGet]
        public ActionResult TotalMovementsReached(Guid notificationId)
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult TotalActiveLoadsReached(Guid notificationId)
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult TotalIntendedQuantityReached(Guid notificationId)
        {
            return View();
        }

        [HttpGet]
        public ActionResult TotalIntendedQuantityExceeded(Guid notificationId)
        {
            return View();
        }

        [HttpGet]
        public ActionResult ConsentPeriodExpired(Guid notificationId)
        {
            return View();
        }

        [HttpGet]
        public ActionResult ConsentExpiresInFourWorkingDays(Guid notificationId)
        {
            return View();
        }

        [HttpGet]
        public ActionResult ConsentExpiresInThreeOrLessWorkingDays(Guid notificationId)
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> RedirectToShipmentDate(Guid notificationId)
        {
            return await ReturnShipmentDateView(notificationId);
        }

        [HttpGet]
        public ActionResult ConsentWithdrawn(Guid notificationId)
        {
            return View();
        }
    }
}