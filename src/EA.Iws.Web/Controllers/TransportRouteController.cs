namespace EA.Iws.Web.Controllers
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
            using (var client = apiClient())
            {
                var summary = await client.SendAsync(User.GetAccessToken(), new GetTransportRouteSummaryForNotification(id));

                return View(summary);
            }
        } 
    }
}