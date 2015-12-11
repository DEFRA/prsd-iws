namespace EA.Iws.Web.Areas.AdminImportAssessment.Controllers
{
    using System;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;

    [Authorize(Roles = "internal")]
    public class HomeController : Controller
    {
        private readonly IMediator mediator;

        public HomeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult InternalMenu(Guid id)
        {
            var data = mediator.SendAsync(new GetNotificationDetails(id)).GetAwaiter().GetResult();

            return PartialView("_InternalMenu", data);
        }
    }
}