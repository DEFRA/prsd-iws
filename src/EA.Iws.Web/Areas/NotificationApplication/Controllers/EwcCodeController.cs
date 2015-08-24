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
    using ViewModels.EwcCode;
    using ViewModels.WasteCodes;

    [Authorize]
    public class EwcCodeController : BaseWasteCodeController
    {
        private static readonly IList<CodeType> ewcCodeTypes = new[] { CodeType.Ewc }; 
        private readonly IMap<WasteCodeDataAndNotificationData, EwcCodeViewModel> mapper;

        public EwcCodeController(Func<IIwsClient> apiClient, 
            IMap<WasteCodeDataAndNotificationData, EwcCodeViewModel> mapper) : base(apiClient, CodeType.Ewc)
        {
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, bool backToOverview = false)
        {
            using (var client = ApiClient())
            {
                var result =
                    await
                        client.SendAsync(User.GetAccessToken(),
                            new GetWasteCodeLookupAndNotificationDataByTypes(id, ewcCodeTypes, ewcCodeTypes));

                return View(mapper.Map(result));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, EwcCodeViewModel model, string command, string remove, bool backToOverview = false)
        {
            return await Post(id, model, command, remove, backToOverview);
        } 

        protected override async Task<ActionResult> ContinueAction(Guid id, BaseWasteCodeViewModel viewModel, bool backToOverview)
        {
            using (var client = ApiClient())
            {
                await
                    client.SendAsync(User.GetAccessToken(),
                        new SetEwcCodes(id, viewModel.EnterWasteCodesViewModel.SelectedWasteCodes,
                            viewModel.EnterWasteCodesViewModel.IsNotApplicable));

                return (backToOverview) ? BackToOverviewResult(id) 
                    : RedirectToAction("Index", "YCode", new { id });
            }
        }
    }
}