namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.ImportNotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using ViewModels.Home;

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
            var details = await mediator.SendAsync(new GetSummary(id));

            var model = new SummaryTableContainerViewModel
            {
                Details = details,
                ShowChangeLinks = details.Status == ImportNotificationStatus.NotificationReceived
            };

            return View(model);
        }
    }
}