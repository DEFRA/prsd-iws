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
    using ViewModels.HCode;
    using ViewModels.WasteCodes;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class HCodeController : BaseWasteCodeController
    {
        private readonly IMap<WasteCodeDataAndNotificationData, HCodeViewModel> mapper;

        private static readonly IList<CodeType> codeTypes = new[] { CodeType.H }; 

        public HCodeController(IMediator mediator, IMap<WasteCodeDataAndNotificationData, HCodeViewModel> mapper) : base(mediator, CodeType.H)
        {
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var result =
                await
                    Mediator.SendAsync(new GetWasteCodeLookupAndNotificationDataByTypes(id, codeTypes, codeTypes));

            return View(mapper.Map(result));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, HCodeViewModel model, string command, string remove, bool backToOverview = false)
        {
            return await Post(id, model, command, remove, backToOverview);
        } 

        protected override async Task<ActionResult> ContinueAction(Guid id, BaseWasteCodeViewModel viewModel, bool backToOverview)
        {
            await
                Mediator.SendAsync(new SetHCodes(id, viewModel.EnterWasteCodesViewModel.SelectedWasteCodes,
                        viewModel.EnterWasteCodesViewModel.IsNotApplicable));

            return (backToOverview) ? BackToOverviewResult(id) 
                : RedirectToAction("Index", "UnClass", new { id });
        }
    }
}