namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.ImportNotificationAssessment;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using ViewModels.Home;

    [Authorize(Roles = "internal")]
    public class HomeController : Controller
    {
        private readonly AuthorizationService authorizationService;
        private readonly IMediator mediator;

        public HomeController(IMediator mediator, AuthorizationService authorizationService)
        {
            this.mediator = mediator;
            this.authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var details = await mediator.SendAsync(new GetSummary(id));
            var showLink = Task.Run(() => authorizationService.AuthorizeActivity(typeof(GetOriginalNumberOfShipments))).Result;

            var model = new SummaryTableContainerViewModel
            {
                Details = details,
                ShowChangeLinks = details.Status == ImportNotificationStatus.NotificationReceived,
                ShowChangeShipmentNumberLink = showLink
            };

            return View(model);
        }
    }
}