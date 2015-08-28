namespace EA.Iws.Web.Areas.MovementDocument.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Movement;
    using ViewModels;

    [Authorize]
    public class ShipmentDateController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public ShipmentDateController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid movementId)
        {
            using (var client = apiClient())
            {
                var movementDateInfo = await client.SendAsync(User.GetAccessToken(), new GetShipmentDateDataByMovementId(movementId));

                var model = new ShipmentDateViewModel(movementDateInfo);

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(ShipmentDateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    await client.SendAsync(User.GetAccessToken(), model.ToRequest());

                    return RedirectToAction("Index", "Quantity", new { movementId = model.MovementId });
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
}