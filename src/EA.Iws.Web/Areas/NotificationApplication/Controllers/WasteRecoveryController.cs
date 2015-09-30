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
        public async Task<ActionResult> Percentage(Guid id, bool? backToOverview = null)
        {
            //TODO: mediator call

            return View();
        }
    }
}