namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Requests.Movement;
    using Requests.MovementReceipt;
    using ViewModels;

    [Authorize]
    public class HomeController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public HomeController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Navigation(Guid id)
        {
            using (var client = apiClient())
            {
                var result =
                    client.SendAsync(User.GetAccessToken(),
                            new GetMovementProgressInformation(id))
                            .GetAwaiter()
                            .GetResult();

                return PartialView("_Navigation", result);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult ReceiptSummary(Guid id)
        {
            using (var client = apiClient())
            {
                var result = client.SendAsync(User.GetAccessToken(), new GetMovementReceiptSummaryDataByMovementId(id)).GetAwaiter().GetResult();

                return PartialView("_ReceiptSummary", result);
            }
        }
    }
}