namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Requests.TransportRoute;

    [Authorize]
    public class TransportRouteController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public TransportRouteController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Summary(Guid id)
        {
            ViewBag.NotificationId = id;

            using (var client = apiClient())
            {
                var summary = await client.SendAsync(User.GetAccessToken(), new GetTransportRouteSummaryForNotification(id));

                return View(summary);
            }
        }

        [HttpPost]
        public ActionResult Summary(Guid id, FormCollection model)
        {
            return RedirectToAction("Index", "Shipment", new { id });
        }
    }
}