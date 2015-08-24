namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.WasteCodes;
    using Infrastructure;
    using Prsd.Core.Mapper;
    using Requests.WasteCodes;
    using ViewModels.BaselOecdCode;

    [Authorize]
    public class BaselOecdCodeController : Controller
    {
        private readonly Func<IIwsClient> apiClient;
        private readonly IMap<WasteCodeDataAndNotificationData, BaselOecdCodeViewModel> codeMapper;
        private static readonly IList<CodeType> RequiredCodeTypes = new[] { CodeType.Basel, CodeType.Oecd };

        public BaselOecdCodeController(Func<IIwsClient> apiClient,
            IMap<WasteCodeDataAndNotificationData, BaselOecdCodeViewModel> codeMapper)
        {
            this.apiClient = apiClient;
            this.codeMapper = codeMapper;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, bool backToOverview = false)
        {
            using (var client = apiClient())
            {
                var result =
                    await
                        client.SendAsync(User.GetAccessToken(),
                            new GetWasteCodeLookupAndNotificationDataByTypes(id, RequiredCodeTypes, RequiredCodeTypes));

                return View(codeMapper.Map(result));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, BaselOecdCodeViewModel model, bool backToOverview = false)
        {
            using (var client = apiClient())
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var codeType = (model.SelectedCode.HasValue)
                    ? model.WasteCodes.Single(wc => wc.Id == model.SelectedCode.Value).CodeType
                    : CodeType.Basel;

                await
                    client.SendAsync(User.GetAccessToken(),
                        new SetBaselOecdCodeForNotification(id,
                            codeType,
                            model.NotListed,
                            model.SelectedCode));

                return (backToOverview) ? RedirectToAction("Index", "Home", new { id }) 
                    : RedirectToAction("Index", "EwcCode", new { id });
            }
        }
    }
}