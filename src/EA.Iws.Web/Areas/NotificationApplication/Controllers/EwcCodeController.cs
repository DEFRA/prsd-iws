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
    [NotificationReadOnlyFilter]
    public class EwcCodeController : BaseWasteCodeController
    {
        private static readonly IList<CodeType> ewcCodeTypes = new[] { CodeType.Ewc }; 
        private readonly IMap<WasteCodeDataAndNotificationData, EwcCodeViewModel> mapper;
        private readonly Func<IIwsClient> apiClient;

        public EwcCodeController(Func<IIwsClient> apiClient, 
            IMap<WasteCodeDataAndNotificationData, EwcCodeViewModel> mapper) : base(apiClient, CodeType.Ewc)
        {
            this.mapper = mapper;
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, bool backToOverview = false)
        {
            using (var client = apiClient())
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
        public async Task<ActionResult> Index(Guid id, EwcCodeViewModel model, string command, string remove,
            bool backToOverview = false)
        {
            return await Post(id, model, command, remove, backToOverview);
        }

        protected override async Task<ActionResult> ContinueAction(Guid id, BaseWasteCodeViewModel viewModel, bool backToOverview)
        {
            using (var client = ApiClient())
            {
                await
                    client.SendAsync(User.GetAccessToken(),
                        new SetEwcCodes(id, viewModel.EnterWasteCodesViewModel.SelectedWasteCodes));

                return (backToOverview)
                    ? BackToOverviewResult(id)
                    : RedirectToAction("Index", "YCode", new { id });
            }
        }
    }
}