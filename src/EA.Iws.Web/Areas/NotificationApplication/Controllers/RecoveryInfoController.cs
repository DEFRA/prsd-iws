namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Shared;
    using Prsd.Core.Mediator;
    using Requests.RecoveryInfo;
    using ViewModels.RecoveryInfo;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class RecoveryInfoController : Controller
    {
        private readonly IMediator mediator;

        public RecoveryInfoController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, bool? backToOverview = null)
        {
            var result = await mediator.SendAsync(new GetRecoveryInfoProvider(id));

            return View(new RecoveryInfoViewModel(result));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, RecoveryInfoViewModel model, bool? backToOverview = null)
        {
            await mediator.SendAsync(new SetRecoveryInfoProvider(model.ProvidedBy.Value, id));

            return model.ProvidedBy == ProvidedBy.Notifier
                ? RedirectToAction("RecoveryPercentage", "RecoveryInfo")
                : RedirectToAction("Index", "Home");
        }
    }
}