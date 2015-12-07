namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement.Capture;
    using ViewModels.Capture;

    [Authorize(Roles = "internal")]
    public class CaptureController : Controller
    {
        private const string NumberKey = "Number";
        private readonly IMediator mediator;

        public CaptureController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Index(Guid id)
        {
            return View(new SearchViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, SearchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await mediator.SendAsync(new GetImportMovementIdIfExists(id, model.Number.Value));

            if (!result.HasValue)
            {
                TempData[NumberKey] = model.Number.Value;
                return RedirectToAction("Create");
            }

            throw new NotImplementedException();
        }

        [HttpGet]
        public ActionResult Create(Guid id)
        {
            object numberData;
            int number;
            if (!TempData.TryGetValue(NumberKey, out numberData) 
                || !int.TryParse(numberData.ToString(), out number))
            {
                return RedirectToAction("Index");
            }

            return View(new CreateViewModel
            {
                Number = number
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Guid id, CreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.SendAsync(new CreateImportMovement(id,
                model.Number,
                model.ActualShipmentDate.AsDateTime().Value,
                model.PrenotificationDate.AsDateTime()));

            throw new NotImplementedException();
        }
    }
}