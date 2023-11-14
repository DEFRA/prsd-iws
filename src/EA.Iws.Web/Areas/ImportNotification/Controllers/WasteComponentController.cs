namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using EA.Iws.Core.ImportNotification.Draft;
    using EA.Iws.Web.Areas.ImportNotification.ViewModels.WasteComponent;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;

    [AuthorizeActivity(typeof(SetDraftData<>))]
    public class WasteComponentController : Controller
    {
        private readonly IMediator mediator;

        public WasteComponentController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var details = await mediator.SendAsync(new GetNotificationDetails(id));
            var data = await mediator.SendAsync(new GetDraftData<WasteComponent>(id));
            var model = new WasteComponentViewModel(details, data);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, WasteComponentViewModel model)
        {
            var wasteOperation = new WasteComponent(id)
            {
                WasteComponentTypes = model.SelectedCodes.ToArray()
            };

            if (wasteOperation.WasteComponentTypes != null && wasteOperation.WasteComponentTypes.Count() > 0)
            {
                await mediator.SendAsync(new SetDraftData<WasteComponent>(id, wasteOperation));
            }

            return RedirectToAction("Index", "WasteCodes");
        }
    }
}