namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.ImportNotification.Draft;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using ViewModels.Shipment;

    [Authorize(Roles = "internal")]
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
            var data = mapper.Map<Shipment>(model, id);

            await mediator.SendAsync(new SetDraftData<Shipment>(id, data));

            return RedirectToAction("Index", "WasteOperation", new { id });
        }
    }
}