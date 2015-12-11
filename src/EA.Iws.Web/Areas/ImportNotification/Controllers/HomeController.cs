namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Threading.Tasks;
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

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var model = await mediator.SendAsync(new GetSummary(id));

            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult _InternalMenuImport(Guid id)
        {
            var response = mediator.SendAsync(new GetNotificationDetails(id)).GetAwaiter().GetResult();

            return PartialView(response);
        }
    }
}