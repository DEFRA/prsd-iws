namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.Carriers;
    using Infrastructure;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Carriers;
    using ViewModels.Carrier;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class CarrierController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public CarrierController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Add(Guid id, bool? backToOverview = null)
        {
            ViewBag.BackToOverview = backToOverview.GetValueOrDefault();
            var model = new AddCarrierViewModel { NotificationId = id };
            await this.BindCountryList(apiClient);
            model.Address.DefaultCountryId = this.GetDefaultCountryId();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(AddCarrierViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(apiClient);
                return View(model);
            }
            using (var client = apiClient())
            {
                try
                {
                    await client.SendAsync(User.GetAccessToken(), model.ToRequest());
                    
                    return RedirectToAction("List", "Carrier", new { id = model.NotificationId, backToOverview });
                }
                catch (ApiBadRequestException ex)
                {
                    this.HandleBadRequest(ex);
                    if (ModelState.IsValid)
                    {
                        throw;
                    }
                }

                await this.BindCountryList(client);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id, Guid entityId, bool? backToOverview = null)
        {
            using (var client = apiClient())
            {
                var carrier = await client.SendAsync(User.GetAccessToken(), new GetCarrierForNotification(id, entityId));

                var model = new EditCarrierViewModel(carrier);

                await this.BindCountryList(client);
                model.Address.DefaultCountryId = this.GetDefaultCountryId();
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditCarrierViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                await this.BindCountryList(apiClient);
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    UpdateCarrierForNotification request = model.ToRequest();

                    await client.SendAsync(User.GetAccessToken(), request);

                    return RedirectToAction("List", "Carrier",
                        new { id = model.NotificationId, backToOverview });
                }
                catch (ApiBadRequestException ex)
                {
                    this.HandleBadRequest(ex);

                    if (ModelState.IsValid)
                    {
                        throw;
                    }
                }
                await this.BindCountryList(client);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<ActionResult> List(Guid id, bool? backToOverview = null)
        {
            ViewBag.BackToOverview = backToOverview.GetValueOrDefault();
            var model = new CarrierListViewModel();

            using (var client = apiClient())
            {
                var response =
                    await client.SendAsync(User.GetAccessToken(), new GetCarriersByNotificationId(id));

                model.NotificationId = id;
                model.Carriers = new List<CarrierData>(response);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Remove(Guid id, Guid entityId, bool? backToOverview = null)
        {
            ViewBag.BackToOverview = backToOverview.GetValueOrDefault();

            using (var client = apiClient())
            {
                var carrier = await client.SendAsync(User.GetAccessToken(), new GetCarrierForNotification(id, entityId));

                var model = new RemoveCarrierViewModel
                {
                    CarrierId = carrier.Id,
                    CarrierName = carrier.Business.Name,
                    NotificationId = carrier.NotificationId
                };

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Remove(RemoveCarrierViewModel model, bool? backToOverview = null)
        {
            using (var client = apiClient())
            {
                try
                {
                    await client.SendAsync(User.GetAccessToken(), new DeleteCarrierForNotification(model.NotificationId, model.CarrierId));
                }
                catch (ApiBadRequestException ex)
                {
                    this.HandleBadRequest(ex);
                    if (ModelState.IsValid)
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction("List", "Carrier", new { id = model.NotificationId, backToOverview });
        }
    }
}