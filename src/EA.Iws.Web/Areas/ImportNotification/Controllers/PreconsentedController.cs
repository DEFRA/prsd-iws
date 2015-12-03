namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.ImportNotification.Draft;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using ViewModels.Preconsented;

    [Authorize(Roles = "internal")]
    public class PreconsentedController : Controller
    {
        private readonly IMediator mediator;

        public PreconsentedController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var preconsented = await mediator.SendAsync(new GetDraftData<Preconsented>(id));

            var model = new PreconsentedViewModel(preconsented.PreconsentedFacilityExists);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, PreconsentedViewModel model)
        {
            var preconsented = new Preconsented(id)
            {
                PreconsentedFacilityExists = model.SelectedValue
            };

            await mediator.SendAsync(new SetDraftData<Preconsented>(id, preconsented));

            return RedirectToAction("Index", "Exporter");
        }
    }
}