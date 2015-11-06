namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.ImportNotification.Draft;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using Requests.ImportNotification.WasteType;
    using ViewModels.WasteType;

    [Authorize(Roles = "internal")]
    public class WasteTypeController : Controller
    {
        private readonly IMapper mapper;
        private readonly IMediator mediator;

        public WasteTypeController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var allCodes = await mediator.SendAsync(new GetAllWasteCodes());
            var data = await mediator.SendAsync(new GetDraftData<WasteType>(id));

            var model = mapper.Map<WasteTypeViewModel>(data, allCodes);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, WasteTypeViewModel model)
        {
            var wasteType = mapper.Map<WasteType>(model);

            await mediator.SendAsync(new SetDraftData<WasteType>(id, wasteType));

            return RedirectToAction("Index", "StateOfExport");
        }
    }
}