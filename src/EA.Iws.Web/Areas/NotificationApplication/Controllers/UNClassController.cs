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
    using ViewModels.UNClass;
    using ViewModels.WasteCodes;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class UnClassController : BaseWasteCodeController
    {
        private readonly IMap<WasteCodeDataAndNotificationData, UNClassViewModel> mapper;
        private static readonly IList<CodeType> codeTypes = new[] { CodeType.Un }; 

        public UnClassController(IMediator mediator, IMap<WasteCodeDataAndNotificationData, UNClassViewModel> mapper, IAuditService auditService) : base(mediator, CodeType.Un, auditService)
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
        public async Task<ActionResult> Index(Guid id, UNClassViewModel model, string command, string remove, bool backToOverview = false)
        {
            return await Post(id, model, command, remove, backToOverview);
        }

        protected override async Task<ActionResult> ContinueAction(Guid id, BaseWasteCodeViewModel viewModel, bool backToOverview)
        {
            var existingData = await Mediator.SendAsync(new GetWasteCodeLookupAndNotificationDataByTypes(id, codeTypes, codeTypes));

            await
                Mediator.SendAsync(new SetUNClasses(id, viewModel.EnterWasteCodesViewModel.SelectedWasteCodes,
                        viewModel.EnterWasteCodesViewModel.IsNotApplicable));

            await this.AddAuditEntries(existingData, viewModel, id, "UN classes");

            return (backToOverview) ? BackToOverviewResult(id) 
                : RedirectToAction("Index", "UnNumber", new { id });
        }
    }
}