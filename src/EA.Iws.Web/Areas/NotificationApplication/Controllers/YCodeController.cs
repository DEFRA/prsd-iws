namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.WasteCodes;
    using Infrastructure;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.WasteCodes;
    using ViewModels.WasteCodes;
    using ViewModels.YCode;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class YCodeController : BaseWasteCodeController
    {
        private readonly IMap<WasteCodeDataAndNotificationData, YCodeViewModel> mapper;
        private static readonly IList<CodeType> RequiredCodeTypes = new[] { CodeType.Y };

        public YCodeController(IMediator mediator, IMap<WasteCodeDataAndNotificationData, YCodeViewModel> mapper)
            : base(mediator, CodeType.Y)
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
        public async Task<ActionResult> Index(Guid id, YCodeViewModel model, string command, string remove, bool backToOverview = false)
        {
            return await Post(id, model, command, remove, backToOverview);
        }

        protected override async Task<ActionResult> ContinueAction(Guid id, BaseWasteCodeViewModel viewModel, bool backToOverview)
        {
            await
                Mediator.SendAsync(new SetYCodes(id, viewModel.EnterWasteCodesViewModel.SelectedWasteCodes,
                        viewModel.EnterWasteCodesViewModel.IsNotApplicable));

            return (backToOverview) ? BackToOverviewResult(id) 
                : RedirectToAction("Index", "HCode", new { id });
        }
    }
}