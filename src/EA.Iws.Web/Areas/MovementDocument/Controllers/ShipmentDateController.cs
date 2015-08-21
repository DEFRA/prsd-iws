namespace EA.Iws.Web.Areas.MovementDocument.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Requests.IntendedShipments;
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
        public async Task<ActionResult> Index(Guid id)
        {
            using (var client = apiClient())
            {
                var notificationShipmentInfo = await client.SendAsync(User.GetAccessToken(), new GetIntendedShipmentInfoForNotification(id));

                var model = new ShipmentDateViewModel
                {
                    StartDate = notificationShipmentInfo.FirstDate,
                    EndDate = notificationShipmentInfo.LastDate
                };

                return View(model);
            }
        }
    }
}