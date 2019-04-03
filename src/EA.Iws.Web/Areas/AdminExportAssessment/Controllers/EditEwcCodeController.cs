namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Notification.Audit;
    using Core.WasteCodes;
    using Infrastructure;
    using Infrastructure.Authorization;
    using NotificationApplication.Controllers;
    using NotificationApplication.ViewModels.WasteCodes;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
    using Requests.WasteCodes;
    using ViewModels.EditEwcCode;

    [AuthorizeActivity(typeof(UpdateEwcCode))]
    public class EditEwcCodeController : BaseWasteCodeController
    {
        private static readonly IList<CodeType> ewcCodeTypes = new[] { CodeType.Ewc };
        private readonly IMap<WasteCodeDataAndNotificationData, EditEwcCodeViewModel> mapper;

        public EditEwcCodeController(IMediator mediator,
           IMap<WasteCodeDataAndNotificationData, EditEwcCodeViewModel> mapper, IAuditService auditService) : base(mediator, CodeType.Ewc, auditService)
        {
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var result = await Mediator.SendAsync(new GetWasteCodeLookupAndNotificationDataByTypes(id, ewcCodeTypes, ewcCodeTypes));
            return View(mapper.Map(result));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, EditEwcCodeViewModel model, string command, string remove,
    bool backToOverview = false)
        {
            return await Post(id, model, command, remove, backToOverview);
        }

        protected override async Task<ActionResult> ContinueAction(Guid id, BaseWasteCodeViewModel viewModel, bool backToOverview = false)
        {
            var existingData = await Mediator.SendAsync(new GetWasteCodeLookupAndNotificationDataByTypes(id, ewcCodeTypes, ewcCodeTypes));

            await
                Mediator.SendAsync(new UpdateEwcCode(id, viewModel.EnterWasteCodesViewModel.SelectedWasteCodes));

            await this.AddAuditEntries(existingData, viewModel, id, NotificationAuditScreenType.EwcCodes);

            return RedirectToAction("Index", "Overview");
        }
    }
}