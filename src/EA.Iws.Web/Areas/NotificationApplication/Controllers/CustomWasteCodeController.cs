namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using Core.Notification.Audit;
    using Core.Shared;
    using Core.WasteCodes;
    using Infrastructure;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Requests.WasteCodes;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.CustomWasteCode;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class CustomWasteCodeController : Controller
    {
        private readonly IMediator mediator;
        private readonly IMap<WasteCodeDataAndNotificationData, CustomWasteCodesViewModel> mapper;
        private readonly IAuditService auditService;

        public CustomWasteCodeController(IMediator mediator, IMap<WasteCodeDataAndNotificationData, CustomWasteCodesViewModel> mapper, IAuditService auditService)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.auditService = auditService;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var result = await mediator.SendAsync(
                        new GetWasteCodeLookupAndNotificationDataByTypes(id, null, new[]
                            {
                                CodeType.ExportCode,
                                CodeType.ImportCode,
                                CodeType.CustomsCode,
                                CodeType.OtherCode
                            }));

            return View(mapper.Map(result));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, CustomWasteCodesViewModel model, bool backToOverview = false)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingData = await mediator.SendAsync(
                        new GetWasteCodeLookupAndNotificationDataByTypes(id, null, new[]
                            {
                                CodeType.ExportCode,
                                CodeType.ImportCode,
                                CodeType.CustomsCode,
                                CodeType.OtherCode
                            }));

            await
                mediator.SendAsync(new SetCustomWasteCodes(id,
                        model.ExportNationalCode,
                        model.ExportNationalCodeNotApplicable,
                        model.ImportNationalCode,
                        model.ImportNationalCodeNotApplicable,
                        model.CustomsCode,
                        model.CustomsCodeNotApplicable,
                        model.OtherCode,
                        model.OtherCodeNotApplicable));

            await this.auditService.AddAuditEntry(this.mediator,
                      id,
                      User.GetUserId(),
                      existingData.NotificationWasteCodeData[CodeType.ExportCode].Count() == 0 ? NotificationAuditType.Added : NotificationAuditType.Updated,
                      NotificationAuditScreenType.OtherCodes);

            var notificationInfo = await mediator.SendAsync(new GetNotificationBasicInfo(id));

            if (notificationInfo.NotificationType == NotificationType.Recovery)
            {
                return (backToOverview) ? RedirectToAction("Index", "Home", new { id })
                    : RedirectToAction("Index", "WasteRecovery", new { id });
            }
            return RedirectToAction("Index", "Home", new { id });
        }
    }
}