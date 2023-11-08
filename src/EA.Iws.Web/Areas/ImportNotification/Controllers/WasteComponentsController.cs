namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using EA.Iws.Core.ImportNotification.Draft;
    using EA.Iws.Web.Areas.ImportNotification.ViewModels.WasteComponents;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;

    [AuthorizeActivity(typeof(SetDraftData<>))]
    public class WasteComponentsController : Controller
    {
        private readonly IMediator mediator;

        public WasteComponentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var details = await mediator.SendAsync(new GetNotificationDetails(id));
            var data = await mediator.SendAsync(new GetDraftData<WasteComponents>(id));
            var model = new WasteComponentsViewModel(details, data);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, WasteComponentsViewModel model)
        {
            var wasteOperation = new WasteComponents(id)
            {
                WasteComponentTypes = model.SelectedCodes.ToArray()
            };

            if (wasteOperation.WasteComponentTypes != null && wasteOperation.WasteComponentTypes.Count() > 0)
            {
                await mediator.SendAsync(new SetDraftData<WasteComponents>(id, wasteOperation));
            }

            return RedirectToAction("Index", "WasteCodes");
        }
    }
}