namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.IntendedShipments;
    using ViewModels.Shipment;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class ShipmentController : Controller
    {
        private readonly IMediator mediator;

        public ShipmentController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var shipmentData =
                await
                    mediator.SendAsync(new GetIntendedShipmentInfoForNotification(id));

            var model = new ShipmentInfoViewModel(shipmentData);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, ShipmentInfoViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await mediator.SendAsync(model.ToRequest(id));

            if (backToOverview.GetValueOrDefault())
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "ChemicalComposition");
        }
    }
}