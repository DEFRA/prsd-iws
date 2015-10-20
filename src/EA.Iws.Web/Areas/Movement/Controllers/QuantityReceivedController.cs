namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.MovementReceipt;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using ViewModels.Quantity;

    public class QuantityReceivedController : Controller
    {
        private readonly IMediator mediator;
        private const string DateReceivedKey = "DateReceived";
        private const string DecisionKey = "Decision";
        private const string UnitKey = "Unit";
        private const string QuantityKey = "Quantity";

        public QuantityReceivedController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            object dateReceivedResult;
            object decisionResult;

            if (TempData.TryGetValue(DateReceivedKey, out dateReceivedResult) && TempData.TryGetValue(DecisionKey, out decisionResult))
            {
                var units = await mediator.SendAsync(new GetMovementUnitsByMovementId(id));

                return View(new QuantityReceivedViewModel
                {
                    DateReceived = DateTime.Parse(dateReceivedResult.ToString()),
                    Decision = (Decision)decisionResult,
                    Unit = units
                });
            }

            return RedirectToAction("Index", "DateReceived", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Guid id, QuantityReceivedViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData[DateReceivedKey] = model.DateReceived;
            TempData[DecisionKey] = model.Decision;
            TempData[UnitKey] = model.Unit;
            TempData[QuantityKey] = model.Quantity;

            return RedirectToAction("Index", "ReceiptComplete", new { id });
        }
    }
}