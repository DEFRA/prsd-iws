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
    using ViewModels.WasteCodes;

    [Authorize]
    public class YCodeController : BaseWasteCodeController
    {
        private readonly IMap<WasteCodeDataAndNotificationData, YCodesViewModel> mapper;
        private static readonly IList<CodeType> RequiredCodeTypes = new[] { CodeType.Y };

        public YCodeController(Func<IIwsClient> apiClient, IMap<WasteCodeDataAndNotificationData, YCodesViewModel> mapper)
            : base(apiClient, CodeType.Y)
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
                            new GetWasteCodeLookupAndNotificationDataByTypes(id, RequiredCodeTypes, RequiredCodeTypes));

                return View(mapper.Map(result));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, YCodesViewModel model, string command, string remove)
        {
            return await Post(id, model, command, remove);
        }

        protected override async Task<ActionResult> ContinueAction(Guid id, BaseWasteCodeViewModel viewModel)
        {
            using (var client = ApiClient())
            {
                await
                    client.SendAsync(User.GetAccessToken(),
                        new SetYCodes(id, viewModel.EnterWasteCodesViewModel.SelectedWasteCodes,
                            viewModel.EnterWasteCodesViewModel.IsNotApplicable));

                return RedirectToAction("Index", new { id });
            }
        }
    }
}