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
    using ViewModels.UNNumber;
    using ViewModels.WasteCodes;

    [Authorize]
    public class UNNumberController : BaseWasteCodeController
    {
        private readonly IMap<WasteCodeDataAndNotificationData, UNNumberViewModel> mapper;
        private readonly IList<CodeType> requiredCodeTypes = new[] { CodeType.UnNumber }; 

        public UNNumberController(Func<IIwsClient> apiClient, IMap<WasteCodeDataAndNotificationData, UNNumberViewModel> mapper) 
            : base(apiClient, CodeType.UnNumber)
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
                            new GetWasteCodeLookupAndNotificationDataByTypes(id, requiredCodeTypes, requiredCodeTypes));

                return View(mapper.Map(result));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, UNNumberViewModel model, string command, string remove, bool backToOverview = false)
        {
            return await Post(id, model, command, remove, backToOverview);
        }

        protected override async Task<ActionResult> ContinueAction(Guid id, BaseWasteCodeViewModel viewModel, bool backToOverview)
        {
            using (var client = ApiClient())
            {
                await
                    client.SendAsync(User.GetAccessToken(),
                        new SetUNNumbers(id, viewModel.EnterWasteCodesViewModel.SelectedWasteCodes,
                            viewModel.EnterWasteCodesViewModel.IsNotApplicable));

                return (backToOverview) ? BackToOverviewResult(id) 
                    : RedirectToAction("Index", "CustomWasteCode", new { id });
            }
        }
    }
}