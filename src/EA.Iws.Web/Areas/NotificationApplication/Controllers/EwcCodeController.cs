namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using Core.Notification.Audit;
    using Core.WasteCodes;
    using Infrastructure;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.WasteCodes;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.EwcCode;
    using ViewModels.WasteCodes;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class EwcCodeController : BaseWasteCodeController
    {
        private static readonly IList<CodeType> ewcCodeTypes = new[] { CodeType.Ewc };
        private readonly IMap<WasteCodeDataAndNotificationData, EwcCodeViewModel> mapper;

        public EwcCodeController(IMediator mediator,
            IMap<WasteCodeDataAndNotificationData, EwcCodeViewModel> mapper, IAuditService auditService) : base(mediator, CodeType.Ewc, auditService)
        {
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, bool backToOverview = false)
        {
            var result =
                await
                    Mediator.SendAsync(new GetWasteCodeLookupAndNotificationDataByTypes(id, ewcCodeTypes, ewcCodeTypes));

            return View(mapper.Map(result));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, EwcCodeViewModel model, string command, string remove,
            bool backToOverview = false)
        {
            return await Post(id, model, command, remove, backToOverview);
        }

        protected override async Task<ActionResult> ContinueAction(Guid id, BaseWasteCodeViewModel viewModel, bool backToOverview)
        {
            var existingData = await Mediator.SendAsync(new GetWasteCodeLookupAndNotificationDataByTypes(id, ewcCodeTypes, ewcCodeTypes));

            await
                Mediator.SendAsync(new SetEwcCodes(id, viewModel.EnterWasteCodesViewModel.SelectedWasteCodes));

            await this.AddAuditEntries(existingData, viewModel, id, NotificationAuditScreenType.EwcCodes);

            return (backToOverview)
                ? BackToOverviewResult(id)
                : RedirectToAction("Index", "YCode", new { id });
        }
    }
}