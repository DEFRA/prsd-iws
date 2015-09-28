namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Requests.TransportRoute;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class TransportRouteController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public TransportRouteController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Summary(Guid id, bool? backToOverview = null)
        {
            ViewBag.NotificationId = id;
            ViewBag.BackToOverview = backToOverview.GetValueOrDefault();

            using (var client = apiClient())
            {
                var summary = await client.SendAsync(User.GetAccessToken(), new GetTransportRouteSummaryForNotification(id));

                return View(summary);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Summary(Guid id, FormCollection model, bool? backToOverview = null)
        {
            if (backToOverview.GetValueOrDefault())
            {
                return RedirectToAction("Index", "Home", new { id });
            }
            else
            {
                return RedirectToAction("Index", "CustomsOffice", new { id }); 
            }
        }
    }
}