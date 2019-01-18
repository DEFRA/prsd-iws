namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using Core.MeansOfTransport;
    using Core.Notification.Audit;
    using Infrastructure;
    using Prsd.Core.Helpers;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.MeansOfTransport;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.MeansOfTransport;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class MeansOfTransportController : Controller
    {
        private readonly IMediator mediator;
        private readonly IAuditService auditService;

        public MeansOfTransportController(IMediator mediator, IAuditService auditService)
        {
            this.mediator = mediator;
            this.auditService = auditService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, bool? backToOverview = null)
        {
            var model = new MeansOfTransportViewModel();

            var currentMeans =
                await mediator.SendAsync(new GetMeansOfTransportByNotificationId(id));

            if (currentMeans.Count != 0)
            {
                model.SelectedMeans = string.Join("-", currentMeans.Select(EnumHelper.GetShortName));
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, MeansOfTransportViewModel model, bool? backToOverview = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var currentMeans = await mediator.SendAsync(new GetMeansOfTransportByNotificationId(id));

                var meansList = model.SelectedMeans.Split('-')
                    .Select(MeansOfTransportHelper.GetTransportMethodFromToken)
                    .ToList();

                await this.auditService.AddAuditEntry(this.mediator,
                    id,
                    User.GetUserId(),
                    currentMeans.Count == 0 ? NotificationAuditType.Create : NotificationAuditType.Update,
                    NotificationAuditScreenType.MeansOfTransport);

                await mediator.SendAsync(new SetMeansOfTransportForNotification(id, meansList));
            }
            catch (ApiBadRequestException ex)
            {
                this.HandleBadRequest(ex);

                if (ModelState.IsValid)
                {
                    throw;
                }
            }

            if (backToOverview.GetValueOrDefault())
            {
                return RedirectToAction("Index", "Home", new { id });
            }

            return RedirectToAction("Index", "PackagingTypes", new { id });
        }
    }
}