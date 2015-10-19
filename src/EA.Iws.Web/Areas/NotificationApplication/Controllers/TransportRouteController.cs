namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure;
    using Prsd.Core.Mediator;
    using Requests.TransportRoute;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class TransportRouteController : Controller
    {
        private readonly IMediator mediator;

        public TransportRouteController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Summary(Guid id, bool? backToOverview = null)
        {
            ViewBag.NotificationId = id;
            ViewBag.BackToOverview = backToOverview.GetValueOrDefault();

            var summary = await mediator.SendAsync(new GetTransportRouteSummaryForNotification(id));

            return View(summary);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Summary(Guid id, FormCollection model, bool? backToOverview = null)
        {
            if (backToOverview.GetValueOrDefault())
            {
                return RedirectToAction("Index", "Home", new { id });
            }

            return RedirectToAction("Index", "CustomsOffice", new { id });
        }
    }
}