namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Prsd.Core;
    using Requests.Movement;
    using Requests.MovementReceipt;

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

        [HttpGet]
        public async Task<ActionResult> GenerateMovementDocument(Guid id)
        {
            using (var client = apiClient())
            {
                var result = await client.SendAsync(User.GetAccessToken(), new GenerateMovementDocument(id));

                var downloadName = "IwsMovement" + SystemTime.UtcNow.ToShortDateString() + ".docx";

                return File(result, Prsd.Core.Web.Constants.MicrosoftWordContentType, downloadName);
            }
        } 
    }
}