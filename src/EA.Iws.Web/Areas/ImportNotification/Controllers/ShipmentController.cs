namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.ImportNotification.Draft;
    using EA.Iws.Core.Extensions;
    using EA.Iws.Core.WasteType;
    using EA.Iws.Requests.WasteType;
    using Infrastructure.Authorization;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using ViewModels.Shipment;

    [AuthorizeActivity(typeof(SetDraftData<>))]
    public class ShipmentController : Controller
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public ShipmentController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var data = await mediator.SendAsync(new GetDraftData<Shipment>(id));

            var model = new ShipmentViewModel(data);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, ShipmentViewModel model)
        {
            WasteTypeData wasteTypeData = null;

            try
            {
                wasteTypeData = await mediator.SendAsync(new GetWasteType(id));
            }
            catch (Exception) 
            {
                // This is a test to make sure that if the waste type data cannot be retrieved, the user can still save the shipment data.
                // The waste type data is only used to validate the total shipments field, so if it cannot be retrieved, we will not perform that validation.
            }

            if (model.TotalShipments != null && wasteTypeData != null)
            {
                if ((model.TotalShipments > 1) &&
                    (wasteTypeData.WasteCategoryType == WasteCategoryType.Singleship || 
                     wasteTypeData.WasteCategoryType == WasteCategoryType.Platformrig))
                {
                    ModelState.AddModelError("TotalShipments", "Only one shipment is allowed for Waste Category Type: " + wasteTypeData.WasteCategoryType.GetDisplayName());
                }
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var data = mapper.Map<Shipment>(model, id);

            await mediator.SendAsync(new SetDraftData<Shipment>(id, data));

            return RedirectToAction("Index", "WasteOperation", new { id });
        }
    }
}