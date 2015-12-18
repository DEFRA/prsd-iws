namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Shared;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Create;
    using Requests.WasteRecovery;
    using ViewModels.WasteRecovery;
    
    [Authorize]
    [NotificationReadOnlyFilter]
    public class WasteRecoveryController : Controller
    {
        private readonly IMediator mediator;
        private const string PercentageKey = "Percentage";
        private const string EstimatedValueAmountKey = "EstimatedValueAmount";
        private const string EstimatedValueUnitKey = "EstimatedValueUnit";
        private const string DisposalMethodKey = "DisposalMethod";
        private const string ShipmentInfoUnitsKey = "ShipmentInfoUnits";

        public WasteRecoveryController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, bool? backToOverview = null)
        {
            var result = await mediator.SendAsync(new GetWasteRecoveryProvider(id));

            return View(new WasteRecoveryViewModel(result));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, WasteRecoveryViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.SendAsync(new SetWasteRecoveryProvider(model.ProvidedBy.Value, id));

            return model.ProvidedBy == ProvidedBy.Notifier
                ? RedirectToAction("Percentage", "WasteRecovery", new { backToOverview })
                : RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> Percentage(Guid id, bool? backToOverview = null)
        {
            var percentageRecoverable = await mediator.SendAsync(new GetRecoverablePercentage(id));

            var model = new PercentageViewModel
            {
                PercentageRecoverable = percentageRecoverable
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Percentage(Guid id, PercentageViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData.Add(PercentageKey, model.PercentageRecoverable);

            return RedirectToAction("EstimatedValue", "WasteRecovery", new { backToOverview });
        }

        [HttpGet]
        public async Task<ActionResult> EstimatedValue(Guid id, bool? backToOverview = null)
        {
            object result;
            if (TempData.TryGetValue(PercentageKey, out result))
            {
                var percentageRecoverable = Convert.ToDecimal(result);
                var estimatedValue = await mediator.SendAsync(new GetEstimatedValue(id));
                var shipmentInfoUnits = await GetShipmentInfoUnits(id);

                var model = estimatedValue == null ? new EstimatedValueViewModel(percentageRecoverable, shipmentInfoUnits) 
                    : new EstimatedValueViewModel(percentageRecoverable, estimatedValue, shipmentInfoUnits);
                
                return View(model);
            }

            return RedirectToAction("Percentage", "WasteRecovery", new { backToOverview });
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EstimatedValue(Guid id, EstimatedValueViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData.Add(PercentageKey, model.PercentageRecoverable);
            TempData.Add(EstimatedValueAmountKey, model.Amount.ToMoneyDecimal());
            TempData.Add(EstimatedValueUnitKey, model.SelectedUnits.Value);
            TempData.Add(ShipmentInfoUnitsKey, model.ShipmentInfoUnits);

            return RedirectToAction("RecoveryCost", "WasteRecovery", new { backToOverview });
        }

        [HttpGet]
        public async Task<ActionResult> RecoveryCost(Guid id, bool? backToOverview = null)
        {
            object percentageResult;
            object estimatedValueAmountResult;
            object estimatedValueUnitResult;
            object shipmentInfoUnitsResult;

            if (TempData.TryGetValue(PercentageKey, out percentageResult) 
                && TempData.TryGetValue(EstimatedValueAmountKey, out estimatedValueAmountResult)
                && TempData.TryGetValue(EstimatedValueUnitKey, out estimatedValueUnitResult)
                && TempData.TryGetValue(ShipmentInfoUnitsKey, out shipmentInfoUnitsResult))
            {
                var recoveryCost = await mediator.SendAsync(new GetRecoveryCost(id));
                var estimatedValue = Convert.ToDecimal(estimatedValueAmountResult);
                var unit = (ValuePerWeightUnits)estimatedValueUnitResult;
                var percentage = Convert.ToDecimal(percentageResult);
                var shipmentInfoUnits = (ValuePerWeightUnits)shipmentInfoUnitsResult;

                var model = new RecoveryCostViewModel(
                    percentage,
                    new ValuePerWeightData(estimatedValue, unit),
                    recoveryCost,
                    shipmentInfoUnits);

                return View(model);
            }

            return RedirectToAction("Percentage", "WasteRecovery", new { backToOverview });
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RecoveryCost(Guid id, RecoveryCostViewModel model, bool? backToOverview)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var saveData = new SaveWasteRecovery(id, 
                model.PercentageRecoverable, 
                new ValuePerWeightData(model.EstimatedValueAmount, model.EstimatedValueUnit), 
                new ValuePerWeightData(model.Amount.ToMoneyDecimal(), model.SelectedUnits.Value));

            await mediator.SendAsync(saveData);

            if (model.PercentageRecoverable < 100)
            {
                return RedirectToAction("DisposalMethod", "WasteRecovery");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> DisposalMethod(Guid id, bool? backToOverview = null)
        {
            var model = new DisposalMethodViewModel();
            var disposalMethod = await mediator.SendAsync(new GetDisposalMethod(id));

            if (disposalMethod != null)
            {
                model = new DisposalMethodViewModel(id, disposalMethod);
            }
            else
            {
                model.NotificationId = id;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DisposalMethod(DisposalMethodViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData.Add(DisposalMethodKey, model.DisposalMethod);

            return RedirectToAction("DisposalCost", "WasteRecovery", new { backToOverview });
        }

        [HttpGet]
        public async Task<ActionResult> DisposalCost(Guid id, bool? backToOverview = null)
        {
            object disposalMethodResult;

            if (TempData.TryGetValue(DisposalMethodKey, out disposalMethodResult))
            {
                var costModel = new DisposalCostViewModel();
                var disposalCost = await mediator.SendAsync(new GetDisposalCost(id));

                if (disposalCost != null)
                {
                    costModel = new DisposalCostViewModel(id, disposalCost);
                }
                else
                {
                    var shipmentInfoUnits = await GetShipmentInfoUnits(id);
                    costModel = new DisposalCostViewModel(id, shipmentInfoUnits);
                }

                costModel.DisposalMethod = disposalMethodResult.ToString();

                return View(costModel);
            }

            return RedirectToAction("DisposalMethod", "WasteRecovery", new { backToOverview });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisposalCost(DisposalCostViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.SendAsync(new SetWasteDisposal(model.NotificationId, model.DisposalMethod, model.Amount.ToMoneyDecimal(), model.Units));

            return RedirectToAction("Index", "Home", new { backToOverview });
        }

        private async Task<ValuePerWeightUnits> GetShipmentInfoUnits(Guid id)
        {
            var shipmentUnits = await mediator.SendAsync(new GetShipmentUnits(id));

            var units = ValuePerWeightUnits.Kilogram;
            if (shipmentUnits == ShipmentQuantityUnits.Tonnes)
            {
                units = ValuePerWeightUnits.Tonne;
            }

            return units;
        }
    }
}