namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Shared;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Requests.WasteRecovery;
    using ViewModels.WasteRecovery;
    
    [Authorize]
    [NotificationReadOnlyFilter]
    public class WasteRecoveryController : Controller
    {
        private readonly IMediator mediator;

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
            await mediator.SendAsync(new SetWasteRecoveryProvider(model.ProvidedBy.Value, id));

            return model.ProvidedBy == ProvidedBy.Notifier
                ? RedirectToAction("Percentage", "WasteRecovery", new { backToOverview })
                : RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Percentage(Guid id, bool? backToOverview = null)
        {
            //TODO: mediator call

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> DisposalMethod(Guid id, string amount, int unit, bool? backToOverview = null)
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

            model.Amount = amount;
            model.Units = (ValuePerWeightUnits)unit;

            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisposalMethod(DisposalMethodViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.SendAsync(new SetWasteDisposal(model.NotificationId, model.DisposalMethod, Convert.ToDecimal(model.Amount), model.Units));

            return RedirectToAction("Index", "Home", new { backToOverview });
        }

        [HttpGet]
        public async Task<ActionResult> DisposalCost(Guid id, bool? backToOverview = null)
        {
            var costModel = new DisposalCostViewModel();
            var disposalCost = await mediator.SendAsync(new GetDisposalCost(id));

            if (disposalCost != null)
            {
                costModel = new DisposalCostViewModel(id, disposalCost);
            }
            else
            {
                costModel.NotificationId = id;
            }
            
            return View(costModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DisposalCost(DisposalCostViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("DisposalMethod", "WasteRecovery", new { backToOverview, amount = model.Amount, unit = (int)model.Units});
        }
    }
}