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
    using Requests.WasteCodes;
    using ViewModels.YCode;

    [AuthorizeActivity(typeof(EditYCodes))]
    public class YCodeController : BaseWasteCodeController
    {
        private readonly IMap<WasteCodeDataAndNotificationData, EditYCodeViewModel> mapper;
        private static readonly IList<CodeType> RequiredCodeTypes = new[] { CodeType.Y };

        public YCodeController(IMediator mediator, IMap<WasteCodeDataAndNotificationData,
            EditYCodeViewModel> mapper, IAuditService auditService) : base(mediator, CodeType.Y, auditService)
        {
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var result =
                await
                    Mediator.SendAsync(new GetWasteCodeLookupAndNotificationDataByTypes(id, RequiredCodeTypes, RequiredCodeTypes));

            return View(mapper.Map(result));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, EditYCodeViewModel model, string command, string remove, bool backToOverview = false)
        {
            return await Post(id, model, command, remove, backToOverview);
        }

        protected override async Task<ActionResult> ContinueAction(Guid id, BaseWasteCodeViewModel viewModel, bool backToOverview)
        {
            var existingData = await Mediator.SendAsync(new GetWasteCodeLookupAndNotificationDataByTypes(id, RequiredCodeTypes, RequiredCodeTypes));

            await
                Mediator.SendAsync(new EditYCodes(id, viewModel.EnterWasteCodesViewModel.SelectedWasteCodes,
                        viewModel.EnterWasteCodesViewModel.IsNotApplicable));

            await AddAuditEntries(existingData, viewModel, id, NotificationAuditScreenType.YCodes);

            return RedirectToAction("Index", "Overview");
        }
    }
}