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

    public class EwcCodeController : BaseWasteCodeController
    {
        private static readonly IList<CodeType> ewcCodeTypes = new[] { CodeType.Ewc }; 
        private readonly IMap<WasteCodeDataAndNotificationData, EwcCodeViewModel> mapper;

        public EwcCodeController(Func<IIwsClient> apiClient, 
            IMap<WasteCodeDataAndNotificationData, EwcCodeViewModel> mapper) : base(apiClient)
        {
            this.mapper = mapper;
        }

        public async Task<ActionResult> Index(Guid id)
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

        protected override Task<ActionResult> ContinueAction(BaseWasteCodeViewModel viewModel)
        {
            throw new NotImplementedException();
        }
    }
}