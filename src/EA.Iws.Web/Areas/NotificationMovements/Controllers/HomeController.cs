namespace EA.Iws.Web.Areas.NotificationMovements.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements;

    [AuthorizeActivity(typeof(GetBasicMovementSummary))]
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
    }
}