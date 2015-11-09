namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Create;
    using ViewModels.Create;

    [Authorize]
    public class CreateController : Controller
    {
        private readonly IMediator mediator;
        private const string NumberOfMovementsKey = "NumberOfMovements";
        private const string ShipmentDateKey = "ShipmentDateKey";

        public CreateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Index(Guid notificationId)
        {
            return View(new CreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Guid notificationId, CreateViewModel model)
        {
            TempData[NumberOfMovementsKey] = model.NumberToCreate;

            return RedirectToAction("ShipmentDate", "Create");
        }

        [HttpGet]
        public async Task<ActionResult> ShipmentDate(Guid notificationId)
        {
            object result;
            if (TempData.TryGetValue(NumberOfMovementsKey, out result))
            {
                var numberToCreate = (int)result;
                var shipmentDates = await mediator.SendAsync(new GetShipmentDates(notificationId));

                ViewBag.NumberOfNewMovements = numberToCreate;
                var model = new ShipmentDateViewModel(shipmentDates, numberToCreate);

                return View(model);
            }

            return RedirectToAction("Index", "Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShipmentDate(Guid notificationId, ShipmentDateViewModel model)
        {
            TempData[NumberOfMovementsKey] = model.NumberToCreate;
            TempData[ShipmentDateKey] = model.AsDateTime();

            return RedirectToAction("Quantity", "Create");
        }

        [HttpGet]
        public ActionResult Quantity(Guid notificationId)
        {
            return View();
        }
    }
}