namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using EA.Iws.Core.Notification.Audit;
    using EA.Iws.Requests.Notification;
    using EA.Iws.Requests.NotificationAssessment;
    using EA.Iws.Requests.OperationCodes;
    using EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.OperationCodes;
    using EA.Iws.Web.Infrastructure;
    using EA.Prsd.Core.Mediator;

    public class OperationCodesController : Controller
    {
        private readonly IMediator mediator;
        private readonly IAuditService auditService;

        public OperationCodesController(IMediator mediator, IAuditService auditService)
        {
            this.mediator = mediator;
            this.auditService = auditService;
        }

        [HttpGet]
        public async Task<ActionResult> Edit(Guid id)
        {
            var basicInfo =
                await mediator.SendAsync(new GetNotificationBasicInfo(id));

            var selectedCodes =
                    await mediator.SendAsync(new GetOperationCodesByNotificationId(id));

            OperationCodesViewModel model = new OperationCodesViewModel(basicInfo.NotificationType, selectedCodes);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, OperationCodesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return await Edit(id);
            }

            await mediator.SendAsync(new SetOperationCodes(id, model.SelectedValues));

            await this.auditService.AddAuditEntry(this.mediator,
                    id,
                    User.GetUserId(),
                    NotificationAuditType.Updated,
                    model.NotificationType == Core.Shared.NotificationType.Disposal ? NotificationAuditScreenType.DisposalCodes : NotificationAuditScreenType.RecoveryCodes);

            return RedirectToAction("Index", "Overview");
        }
    }
}