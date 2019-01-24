namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Authorization.Permissions;
    using Core.Notification.Audit;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
    using ViewModels.NumberOfShipments;

    [AuthorizeActivity(ExportNotificationPermissions.CanChangeNumberOfShipmentsOnExportNotification)]
    public class NumberOfShipmentsController : Controller
    {
        private readonly IMediator mediator;
        private readonly IAuditService auditService;

        public NumberOfShipmentsController(IMediator mediator, IAuditService auditService)
        {
            this.mediator = mediator;
            this.auditService = auditService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Guid id, IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("Confirm", model);
        }

        [HttpGet]
        public async Task<ActionResult> Confirm(Guid id, IndexViewModel model)
        {
            var data = await mediator.SendAsync(new GetChangeNumberOfShipmentConfrimationData(id, model.Number.GetValueOrDefault()));
            var confirmModel = new ConfirmViewModel(data);
            confirmModel.NewNumberOfShipments = model.Number.GetValueOrDefault();

            return View(confirmModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Confirm(ConfirmViewModel model)
        {
           await mediator.SendAsync(new SetNewNumberOfShipments(model.NotificationId, model.OldNumberOfShipments, model.NewNumberOfShipments));

            await this.auditService.AddAuditEntry(this.mediator,
                    model.NotificationId,
                    User.GetUserId(),
                    NotificationAuditType.Updated,
                    NotificationAuditScreenType.AmountsAndDates);

            return RedirectToAction("Index", "Overview");
        }
    }
}