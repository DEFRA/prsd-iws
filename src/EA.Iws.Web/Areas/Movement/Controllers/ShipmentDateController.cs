namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Movement;
    using ViewModels;

    [Authorize]
    public class ShipmentDateController : Controller
    {
        private readonly IMediator mediator;

        public ShipmentDateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var movementDateInfo = await mediator.SendAsync(new GetShipmentDateDataByMovementId(id));

            var model = new ShipmentDateViewModel(movementDateInfo);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(ShipmentDateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await mediator.SendAsync(model.ToRequest());
                return RedirectToAction("Index", "Quantity", new { id = model.MovementId });
            }
            catch (ApiBadRequestException ex)
            {
                this.HandleBadRequest(ex);

                if (ModelState.IsValid)
                {
                    throw;
                }
            }

            return View(model);
        }
    }
}