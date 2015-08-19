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
    public class UNClassController : BaseWasteCodeController
    {
        private readonly IMap<WasteCodeDataAndNotificationData, UNClassViewModel> mapper;
        private static readonly IList<CodeType> codeTypes = new[] { CodeType.Un }; 

        public UNClassController(Func<IIwsClient> apiClient, IMap<WasteCodeDataAndNotificationData, UNClassViewModel> mapper) : base(apiClient, CodeType.Un)
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
        public async Task<ActionResult> Index(Guid id, UNClassViewModel model, string command, string remove)
        {
            return await Post(id, model, command, remove);
        }

        protected override async Task<ActionResult> ContinueAction(Guid id, BaseWasteCodeViewModel viewModel)
        {
            using (var client = ApiClient())
            {
                await
                    client.SendAsync(User.GetAccessToken(),
                        new SetUNClasses(id, viewModel.EnterWasteCodesViewModel.SelectedWasteCodes,
                            viewModel.EnterWasteCodesViewModel.IsNotApplicable));

                return RedirectToAction("Index", "UNClass", new { id });
            }
        }
    }
}