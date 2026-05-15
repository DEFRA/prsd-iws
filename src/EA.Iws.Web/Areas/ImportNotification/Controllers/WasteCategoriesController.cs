namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using EA.Iws.Core.Extensions;
    using EA.Iws.Core.IntendedShipments;
    using EA.Iws.Core.WasteType;
    using EA.Iws.Requests.ImportNotification;
    using EA.Iws.Requests.IntendedShipments;
    using EA.Iws.Web.Areas.ImportNotification.ViewModels.WasteCategories;
    using EA.Iws.Web.Infrastructure.Authorization;
    using EA.Prsd.Core.Mediator;

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
            IntendedShipmentData shipmentData = null;
            string shipmentErrorMessage = "Only one shipment is allowed for the selected waste category.";

            try
            {
                shipmentData = await mediator.SendAsync(new GetIntendedShipmentInfoForNotification(id));
            }
            catch (Exception)
            {
                // If the shipment data cannot be retrieved then the validation should not be applied as it cannot be confirmed if there are multiple shipments or not.
            }

            if (shipmentData != null && shipmentData.HasShipmentData)
            {
                if ((shipmentData.NumberOfShipments > 1) &&
                    (model.WasteCategories.SelectedValue == WasteCategoryType.Singleship.GetDisplayName() ||
                     model.WasteCategories.SelectedValue == WasteCategoryType.Platformrig.GetDisplayName()))
                {
                    ModelState.AddModelError("WasteCategoryType.SelectedValue", shipmentErrorMessage);
                }
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var wasteCategory = new Core.ImportNotification.Draft.WasteCategories(id)
            {
                WasteCategoryType = model.GetSelectedWasteCategoryType()
            };

            await mediator.SendAsync(new SetDraftData<Core.ImportNotification.Draft.WasteCategories>(id, wasteCategory));

            return RedirectToAction("Index", "WasteComponent");
        }
    }
}