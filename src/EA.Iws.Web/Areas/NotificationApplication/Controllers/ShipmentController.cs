namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Shipment;
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
        public async Task<ActionResult> Add(Guid id)
        {
            using (var client = apiClient())
            {
                var shipmentData =
                    await
                        client.SendAsync(User.GetAccessToken(),
                            new GetShipmentInfoForNotification(id));

                if (shipmentData.HasShipmentData)
                {
                    return RedirectToAction("Edit", new { id });
                }

                var model = new ShipmentInfoViewModel(shipmentData);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(ShipmentInfoViewModel model)
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

                    return RedirectToAction("ChemicalComposition", "WasteType", new { id = model.NotificationId });
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

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id)
        {
            using (var client = apiClient())
            {
                var shipmentData =
                    await
                        client.SendAsync(User.GetAccessToken(),
                            new GetShipmentInfoForNotification(id));

                if (!shipmentData.HasShipmentData)
                {
                    return RedirectToAction("Add", new { id });
                }

                var model = new ShipmentInfoViewModel(shipmentData);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ShipmentInfoViewModel model)
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

                    return RedirectToAction("NotificationOverview", "NotificationApplication", new { id = model.NotificationId });
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