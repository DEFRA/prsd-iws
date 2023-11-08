namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using EA.Iws.Requests.ImportNotification;
    using EA.Iws.Web.Areas.ImportNotification.ViewModels.WasteCategories;
    using EA.Iws.Web.Infrastructure.Authorization;
    using EA.Prsd.Core.Mediator;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    [AuthorizeActivity(typeof(SetDraftData<>))]
    public class WasteCategoriesController : Controller
    {
        private readonly IMediator mediator;

        public WasteCategoriesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var model = new WasteCategoriesViewModel();

            var extraWasteCategory = await mediator.SendAsync(new GetDraftData<Core.ImportNotification.Draft.WasteCategories>(id));

            if (extraWasteCategory.WasteCategoryType.HasValue)
            {
                model.WasteCategories.SelectedValue = Prsd.Core.Helpers.EnumHelper.GetDisplayName(extraWasteCategory.WasteCategoryType.Value);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, WasteCategoriesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var wasteCategory = new Core.ImportNotification.Draft.WasteCategories(id)
            {
                WasteCategoryType = model.GetSelectedWasteCategoryType()
            };

            await mediator.SendAsync(new SetDraftData<Core.ImportNotification.Draft.WasteCategories>(id, wasteCategory));

            return RedirectToAction("Index", "WasteComponents");
        }
    }
}