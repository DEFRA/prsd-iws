namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Create;
    using ViewModels.Create;

    [Authorize]
    public class CreateController : Controller
    {
        private readonly IMediator mediator;
        private const string MovementNumbersKey = "MovementNumbersKey";
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
        public async Task<ActionResult> Index(Guid notificationId, CreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newMovementNumbers = await mediator.SendAsync(new GenerateMovementNumbers(notificationId, model.NumberToCreate.Value));

            TempData[MovementNumbersKey] = newMovementNumbers;

            return RedirectToAction("ShipmentDate", "Create");
        }

        [HttpGet]
        public async Task<ActionResult> ShipmentDate(Guid notificationId)
        {
            object result;
            if (TempData.TryGetValue(MovementNumbersKey, out result))
            {
                var movementNumbers = (IList<int>)result;
                var shipmentDates = await mediator.SendAsync(new GetShipmentDates(notificationId));

                ViewBag.MovementNumbers = movementNumbers;
                var model = new ShipmentDateViewModel(shipmentDates, movementNumbers);

                return View(model);
            }

            return RedirectToAction("Index", "Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShipmentDate(Guid notificationId, ShipmentDateViewModel model)
        {
            TempData[MovementNumbersKey] = model.MovementNumbers;
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