namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.WasteCodes;
    using Infrastructure;
    using Prsd.Core.Mapper;
    using Requests.WasteCodes;
    using ViewModels.UNClass;
    using ViewModels.WasteCodes;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class UnClassController : BaseWasteCodeController
    {
        private readonly IMap<WasteCodeDataAndNotificationData, UNClassViewModel> mapper;
        private static readonly IList<CodeType> codeTypes = new[] { CodeType.Un }; 

        public UnClassController(Func<IIwsClient> apiClient, IMap<WasteCodeDataAndNotificationData, UNClassViewModel> mapper) : base(apiClient, CodeType.Un)
        {
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            using (var client = ApiClient())
            {
                var result =
                    await
                        client.SendAsync(User.GetAccessToken(),
                            new GetWasteCodeLookupAndNotificationDataByTypes(id, codeTypes, codeTypes));

                return View(mapper.Map(result));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, UNClassViewModel model, string command, string remove, bool backToOverview = false)
        {
            return await Post(id, model, command, remove, backToOverview);
        }

        protected override async Task<ActionResult> ContinueAction(Guid id, BaseWasteCodeViewModel viewModel, bool backToOverview)
        {
            using (var client = ApiClient())
            {
                await
                    client.SendAsync(User.GetAccessToken(),
                        new SetUNClasses(id, viewModel.EnterWasteCodesViewModel.SelectedWasteCodes,
                            viewModel.EnterWasteCodesViewModel.IsNotApplicable));

                return (backToOverview) ? BackToOverviewResult(id) 
                    : RedirectToAction("Index", "UnNumber", new { id });
            }
        }
    }
}