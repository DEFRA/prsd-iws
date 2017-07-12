namespace EA.Iws.Web.Areas.ImportNotification.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Authorization.Permissions;
    using Core.ImportNotificationAssessment;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using ViewModels.Home;
    using Web.ViewModels.Shared;

    [AuthorizeActivity(typeof(GetSummary))]
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
            var canChangeNumberOfShipments = Task.Run(() => authorizationService.AuthorizeActivity(typeof(GetOriginalNumberOfShipments))).Result;
            var canChangeEntryExitPoint = Task.Run(() => authorizationService.AuthorizeActivity(ImportNotificationPermissions.CanChangeImportEntryExitPoint)).Result;

            var model = new SummaryTableContainerViewModel
            {
                Details = details,
                ShowChangeLinks = details.Status == ImportNotificationStatus.NotificationReceived,
                ShowChangeNumberOfShipmentsLink = canChangeNumberOfShipments && details.Status == ImportNotificationStatus.Consented,
                ShowChangeEntryExitPointLink = canChangeEntryExitPoint && details.Status != ImportNotificationStatus.New && details.Status != ImportNotificationStatus.NotificationReceived
            };

            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult NotificationSwitcher(Guid id)
        {
            var response = Task.Run(() => mediator.SendAsync(new GetNotificationDetails(id))).Result;

            return PartialView("_NotificationSwitcher", new NotificationSwitcherViewModel(response.NotificationNumber));
        }
    }
}