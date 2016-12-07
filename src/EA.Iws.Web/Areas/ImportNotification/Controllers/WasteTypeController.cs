namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
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
            var model = new WasteTypeViewModel();

            var chemicalComposition = await mediator.SendAsync(new GetDraftData<Core.ImportNotification.Draft.ChemicalComposition>(id));

            if (chemicalComposition.Composition.HasValue)
            {
                model.ChemicalCompositionType.SelectedValue = Prsd.Core.Helpers.EnumHelper.GetDisplayName(chemicalComposition.Composition.Value);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, WasteTypeViewModel model)
        {
            var chemicalComposition = new Core.ImportNotification.Draft.ChemicalComposition(id)
            {
                Composition = model.GetSelectedChemicalComposition()
            };

            await mediator.SendAsync(new SetDraftData<Core.ImportNotification.Draft.ChemicalComposition>(id, chemicalComposition));

            return RedirectToAction("Index", "WasteCodes");
        }
    }
}