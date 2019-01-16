namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using Core.Notification.Audit;
    using Core.Shared;
    using Core.StateOfExport;
    using Infrastructure;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Requests.StateOfExport;
    using Requests.TransportRoute;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.StateOfExport;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class StateOfExportController : Controller
    {
        private readonly IMediator mediator;
        private readonly IMap<StateOfExportWithTransportRouteData, StateOfExportViewModel> mapper;
        private readonly IAuditService auditService;

        public StateOfExportController(IMediator mediator, IMap<StateOfExportWithTransportRouteData, StateOfExportViewModel> mapper, IAuditService auditService)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.auditService = auditService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, bool? backToOverview = null)
        {
            var stateOfExportSetData = await mediator.SendAsync(new GetStateOfExportWithTransportRouteDataByNotificationId(id));

            var notificationCompetentAuthority = await mediator.SendAsync(new GetUnitedKingdomCompetentAuthorityByNotificationId(id));

            var model = mapper.Map(stateOfExportSetData);
            model.NotificationCompetentAuthority = notificationCompetentAuthority.AsUKCompetantAuthority();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, StateOfExportViewModel model, string submit, bool? backToOverview = null)
        {
            await GetCompetentAuthoritiesAndEntryPoints(mediator, model);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return await SubmitAction(id, model, mediator, backToOverview);
        }

        private async Task<ActionResult> SubmitAction(Guid id, StateOfExportViewModel model, IMediator mediator, bool? backToOverview)
        {
            var stateOfExportSetData = await mediator.SendAsync(new GetStateOfExportWithTransportRouteDataByNotificationId(id));

            await mediator.SendAsync(new SetStateOfExportForNotification(id, model.EntryOrExitPointId.Value));

            await this.auditService.AddAuditEntry(this.mediator,
                    id,
                    User.GetUserId(),
                    stateOfExportSetData.StateOfExport.ExitPoint == null ? NotificationAuditType.Create : NotificationAuditType.Update,
                    "Export route");

            if (backToOverview.GetValueOrDefault())
            {
                return RedirectToAction("Index", "Home", new { id });
            }

            return RedirectToAction("Index", "StateOfImport", new { id });
        }

        private async Task GetCompetentAuthoritiesAndEntryPoints(IMediator mediator, StateOfExportViewModel model)
        {
            if (!model.CountryId.HasValue)
            {
                return;
            }

            var entryPointsAndCompetentAuthorities =
                await
                    mediator.SendAsync(new GetCompetentAuthoritiesAndEntryOrExitPointsByCountryId(model.CountryId.Value));

            model.ExitPoints = new SelectList(entryPointsAndCompetentAuthorities.EntryOrExitPoints, "Id", "Name");
        }
    }
}