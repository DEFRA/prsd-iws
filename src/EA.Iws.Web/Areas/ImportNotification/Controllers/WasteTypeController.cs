namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.ImportNotification.Draft;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using ViewModels.WasteType;

    [Authorize(Roles = "internal")]
    public class WasteTypeController : Controller
    {
        private readonly IMediator mediator;

        public WasteTypeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var data = await mediator.SendAsync(new GetDraftData<WasteType>(id));

            var model = new WasteTypeViewModel(data);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, WasteTypeViewModel model)
        {
            var wasteType = new WasteType
            {
                Name = model.Name
            };

            await mediator.SendAsync(new SetDraftData<WasteType>(id, wasteType));

            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }
    }
}