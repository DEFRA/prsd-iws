namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Create;
    using ViewModels.Create;

    public class CreateController : Controller
    {
        private readonly IMediator mediator;
        private const string CreateMovementKey = "CreateMovementKey";

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
            TempData[CreateMovementKey] = model.NumberToCreate;

            return RedirectToAction("ShipmentDate", "Create");
        }

        [HttpGet]
        public async Task<ActionResult> ShipmentDate(Guid notificationId)
        {
            object result;
            if (TempData.TryGetValue(CreateMovementKey, out result))
            {
                var numberToCreate = (int)result;
                var shipmentDates = await mediator.SendAsync(new GetShipmentDates(notificationId));

                var model = new ShipmentDateViewModel(shipmentDates, numberToCreate);

                return View(model);
            }

            return RedirectToAction("Index", "Create");
        }
    }
}