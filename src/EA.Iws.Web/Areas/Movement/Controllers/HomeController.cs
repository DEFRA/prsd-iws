namespace EA.Iws.Web.Areas.Movement.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web;
    using Requests.Movement;
    using Requests.MovementReceipt;

    [Authorize]
    public class HomeController : Controller
    {
        private readonly IMediator mediator;

        public HomeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Navigation(Guid id)
        {
            var result = mediator.SendAsync(new GetMovementProgressInformation(id))
                            .GetAwaiter()
                            .GetResult();

            return PartialView("_Navigation", result);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult ReceiptSummary(Guid id)
        {
            var result = mediator.SendAsync(new GetMovementReceiptSummaryDataByMovementId(id)).GetAwaiter().GetResult();

            return PartialView("_ReceiptSummary", result);
        }

        [HttpGet]
        public async Task<ActionResult> GenerateMovementDocument(Guid id)
        {
            var result = await mediator.SendAsync(new GenerateMovementDocument(id));

            var downloadName = "IwsMovement" + SystemTime.UtcNow.ToShortDateString() + ".docx";

            return File(result, Constants.MicrosoftWordContentType, downloadName);
        }
    }
}