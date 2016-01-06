namespace EA.Iws.Web.Areas.ExportMovement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Movement;
    using Core.Shared;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Requests.Movement.Receive;
    using ViewModels.Quantity;

    [Authorize]
    public class QuantityReceivedController : Controller
    {
        private readonly IMediator mediator;
        private const string DateReceivedKey = "DateReceived";
        private const string UnitKey = "Unit";
        private const string QuantityKey = "Quantity";
        private const string ToleranceKey = "Tolerance";

        public QuantityReceivedController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            object dateReceivedResult;

            if (TempData.TryGetValue(DateReceivedKey, out dateReceivedResult))
            {
                var units = await mediator.SendAsync(new GetMovementUnitsByMovementId(id));

                return View(new QuantityReceivedViewModel
                {
                    DateReceived = DateTime.Parse(dateReceivedResult.ToString()),
                    Unit = units
                });
            }

            return RedirectToAction("Index", "DateReceived", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, QuantityReceivedViewModel model)
        {
            if (!ModelState.IsValid || !model.Quantity.HasValue)
            {
                return View(model);
            }

            var tolerance =
                await mediator.SendAsync(new DoesQuantityReceivedExceedTolerance(id, model.Quantity.Value, model.Unit));

            TempData[DateReceivedKey] = model.DateReceived;
            TempData[UnitKey] = model.Unit;
            TempData[QuantityKey] = model.Quantity;

            if (tolerance == QuantityReceivedTolerance.AboveTolerance 
                || tolerance == QuantityReceivedTolerance.BelowTolerance)
            {
                TempData[ToleranceKey] = tolerance;
                return RedirectToAction("QuantityAbnormal");
            }

            return RedirectToAction("Index", "ReceiptComplete", new { id });
        }

        [HttpGet]
        public ActionResult QuantityAbnormal(Guid id)
        {
            object date;
            object quantity;
            object unit;
            object tolerance;
            if (TempData.TryGetValue(DateReceivedKey, out date)
                && TempData.TryGetValue(QuantityKey, out quantity)
                && TempData.TryGetValue(UnitKey, out unit)
                && TempData.TryGetValue(ToleranceKey, out tolerance))
            {
                return View(new QuantityAbnormalViewModel
                {
                    Quantity = Convert.ToDecimal(quantity),
                    Tolerance = (QuantityReceivedTolerance)tolerance,
                    Unit = (ShipmentQuantityUnits)unit,
                    DateReceived = Convert.ToDateTime(date)
                });
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QuantityAbnormal(Guid id, QuantityAbnormalViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData[DateReceivedKey] = model.DateReceived;
            TempData[UnitKey] = model.Unit;
            TempData[QuantityKey] = model.Quantity;

            return RedirectToAction("Index", "ReceiptComplete");
        }
    }
}