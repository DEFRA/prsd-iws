namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.IntendedShipments;
    using ViewModels.Shipment;

    [Authorize]
    public class ShipmentController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public ShipmentController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, bool? backToOverview = null)
        {
            using (var client = apiClient())
            {
                var shipmentData =
                    await
                        client.SendAsync(User.GetAccessToken(),
                            new GetIntendedShipmentInfoForNotification(id));
                
                var model = new ShipmentInfoViewModel(shipmentData);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(ShipmentInfoViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    await client.SendAsync(User.GetAccessToken(),
                        model.ToRequest());

                    if (backToOverview.GetValueOrDefault())
                    {
                        return RedirectToAction("Index", "Home", new { id = model.NotificationId });
                    }
                    else
                    {
                        return RedirectToAction("ChemicalComposition", "WasteType", new { id = model.NotificationId }); 
                    }
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