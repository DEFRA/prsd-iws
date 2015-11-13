namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Movement;
    using Core.PackagingType;
    using Core.Shared;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Create;
    using ViewModels.Create;

    [Authorize]
    public class CreateController : Controller
    {
        private readonly IMediator mediator;
        private const string MovementNumbersKey = "MovementNumbersKey";
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
        public ActionResult Index(Guid notificationId)
        {
            return View(new CreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid notificationId, CreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newMovementNumbers = await mediator.SendAsync(new GenerateMovementNumbers(notificationId, model.NumberToCreate.Value));

            TempData[MovementNumbersKey] = newMovementNumbers;

            return RedirectToAction("ShipmentDate", "Create");
        }

        [HttpGet]
        public async Task<ActionResult> ShipmentDate(Guid notificationId)
        {
            object result;
            if (TempData.TryGetValue(MovementNumbersKey, out result))
            {
                var movementNumbers = (IList<int>)result;
                var shipmentDates = await mediator.SendAsync(new GetShipmentDates(notificationId));

                ViewBag.MovementNumbers = movementNumbers;
                var model = new ShipmentDateViewModel(shipmentDates, movementNumbers);

                return View(model);
            }

            return RedirectToAction("Index", "Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShipmentDate(Guid notificationId, ShipmentDateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData[MovementNumbersKey] = model.MovementNumbers;
            TempData[ShipmentDateKey] = model.AsDateTime();

            return RedirectToAction("Quantity", "Create");
        }

        [HttpGet]
        public async Task<ActionResult> Quantity(Guid notificationId)
        {
            object result;
            if (TempData.TryGetValue(MovementNumbersKey, out result))
            {
                var movementNumbers = (IList<int>)result;
                var shipmentUnits = await mediator.SendAsync(new GetShipmentUnits(notificationId));

                ViewBag.MovementNumbers = movementNumbers;
                var model = new QuantityViewModel(shipmentUnits, movementNumbers);

                return View(model);
            }

            return RedirectToAction("Index", "Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Quantity(Guid notificationId, QuantityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData[MovementNumbersKey] = model.MovementNumbers;
            TempData[QuantityKey] = model.Quantity;
            TempData[UnitKey] = model.Units;

            return RedirectToAction("PackagingTypes", "Create");
        }

        [HttpGet]
        public async Task<ActionResult> PackagingTypes(Guid notificationId)
        {
            object result;
            if (TempData.TryGetValue(MovementNumbersKey, out result))
            {
                var movementNumbers = (IList<int>)result;
                var availablePackagingTypes = await mediator.SendAsync(new GetPackagingTypes(notificationId));

                ViewBag.MovementNumbers = movementNumbers;
                var model = new PackagingTypesViewModel(availablePackagingTypes, movementNumbers);

                return View(model);
            }

            return RedirectToAction("Index", "Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PackagingTypes(Guid notificationId, PackagingTypesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData[MovementNumbersKey] = model.MovementNumbers;
            TempData[PackagingTypesKey] = model.SelectedValues;

            return RedirectToAction("NumberOfPackages", "Create");
        }

        [HttpGet]
        public ActionResult NumberOfPackages(Guid notificationId)
        {
            object result;
            if (TempData.TryGetValue(MovementNumbersKey, out result))
            {
                var movementNumbers = (IList<int>)result;

                ViewBag.MovementNumbers = movementNumbers;
                var model = new NumberOfPackagesViewModel(movementNumbers);

                return View(model);
            }

            return RedirectToAction("Index", "Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NumberOfPackages(Guid notificationId, NumberOfPackagesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData[MovementNumbersKey] = model.MovementNumbers;
            TempData[NumberOfPackagesKey] = model.Number;

            return RedirectToAction("NumberOfCarriers", "Create");
        }

        [HttpGet]
        public async Task<ActionResult> NumberOfCarriers(Guid notificationId)
        {
            object result;
            if (TempData.TryGetValue(MovementNumbersKey, out result))
            {
                var movementsNumbers = (IList<int>)result;
                var meansOfTransport = await mediator.SendAsync(new GetMeansOfTransport(notificationId));

                ViewBag.MovementNumbers = movementsNumbers;
                var meansOfTransportViewModel = new MeansOfTransportViewModel { NotificationMeansOfTransport = meansOfTransport };
                var model = new NumberOfCarriersViewModel(meansOfTransportViewModel, movementsNumbers);

                return View(model);
            }

            return RedirectToAction("Index", "Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NumberOfCarriers(Guid notificationId, NumberOfCarriersViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData[MovementNumbersKey] = model.MovementNumbers;
            TempData[NumberOfCarriersKey] = model.Number;

            return RedirectToAction("Carriers", "Create");
        }

        [HttpGet]
        public async Task<ActionResult> Carriers(Guid notificationId)
        {
            object movementNumbersResult;
            object numberOfCarriersResult;
            if (TempData.TryGetValue(MovementNumbersKey, out movementNumbersResult)
                && TempData.TryGetValue(NumberOfCarriersKey, out numberOfCarriersResult))
            {
                var movementNumbers = (IList<int>)movementNumbersResult;
                var numberOfCarriers = (int)numberOfCarriersResult;
                var meansOfTransport = await mediator.SendAsync(new GetMeansOfTransport(notificationId));
                var notificationCarriers = await mediator.SendAsync(new GetCarriers(notificationId));

                ViewBag.MovementNumbers = movementNumbers;
                var meansOfTransportViewModel = new MeansOfTransportViewModel { NotificationMeansOfTransport = meansOfTransport };
                var model = new CarrierViewModel(notificationCarriers, numberOfCarriers, meansOfTransportViewModel, movementNumbers);

                TempData[MovementNumbersKey] = movementNumbers;
                TempData[NumberOfCarriersKey] = numberOfCarriers;

                return View(model);
            }

            return RedirectToAction("Index", "Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Carriers(Guid notificationId, CarrierViewModel model)
        {
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

                await mediator.SendAsync(new CreateMovementAndDetails(notificationId, shipmentDate, newMovementDetails));

                return HttpNotFound();
            }

            return RedirectToAction("Index", "Create");
        }
    }
}