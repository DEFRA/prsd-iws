namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.ImportNotification.Draft;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using ViewModels.WasteOperation;

    [Authorize(Roles = "internal")]
    public class WasteOperationController : Controller
    {
        private readonly IMediator mediator;

        public WasteOperationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var details = await mediator.SendAsync(new GetNotificationDetails(id));
            var data = await mediator.SendAsync(new GetDraftData<WasteOperation>(id));

            var model = new WasteOperationViewModel(details, data);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, WasteOperationViewModel model)
        {
            var wasteOperation = new WasteOperation(id)
            {
                OperationCodes = model.SelectedCodes,
                TechnologyEmployed = model.TechnologyEmployed
            };

            await mediator.SendAsync(new SetDraftData<WasteOperation>(id, wasteOperation));

            return RedirectToAction("Index", "WasteType");
        }
    }
}