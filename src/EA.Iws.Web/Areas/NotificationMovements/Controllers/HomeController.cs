namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Movement;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements;
    using ViewModels.Home;

    [Authorize]
    public class HomeController : Controller
    {
        private readonly IMediator mediator;

        public HomeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Summary(Guid notificationId)
        {
            var result = Task.Run(() => mediator.SendAsync(new GetBasicMovementSummary(notificationId))).Result;

            return PartialView("_Summary", result);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult CreateSummary(Guid notificationId, int movementNumber)
        {
            var result = Task.Run(() => mediator.SendAsync(new GetBasicMovementSummary(notificationId))).Result;

            var model = new CreateSummaryViewModel
            {
                SummaryData = result,
                NewShipmentNumber = movementNumber
            };

            return PartialView("_CreateSummary", model);
        }
    }
}