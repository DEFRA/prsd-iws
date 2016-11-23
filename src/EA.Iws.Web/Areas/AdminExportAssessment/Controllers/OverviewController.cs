namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using NotificationApplication.ViewModels.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Requests.NotificationAssessment;

    [Authorize(Roles = "internal")]
    public class OverviewController : Controller
    {
        private readonly AuthorizationService authorizationService;
        private readonly IMediator mediator;

        public OverviewController(IMediator mediator, AuthorizationService authorizationService)
        {
            this.mediator = mediator;
            this.authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var result = await mediator.SendAsync(new GetNotificationOverviewInternal(id));
            var showLink = Task.Run(() => authorizationService.AuthorizeActivity(typeof(GetOriginalNumberOfShipments))).Result;

            var model = new NotificationOverviewViewModel(result);
            model.AmountsAndDatesViewModel.ShowChangeShipmentNumberLink = showLink;

            return View(model);
        }
    }
}