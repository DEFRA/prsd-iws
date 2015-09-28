namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.Shared;
    using Core.WasteCodes;
    using Infrastructure;
    using Prsd.Core.Mapper;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Notification;
    using Requests.WasteCodes;
    using ViewModels.CustomWasteCode;

    [Authorize]
    [NotificationReadOnlyFilter]
    public class CustomWasteCodeController : Controller
    {
        private readonly Func<IIwsClient> apiClient;
        private readonly IMap<WasteCodeDataAndNotificationData, CustomWasteCodesViewModel> mapper;

        public CustomWasteCodeController(Func<IIwsClient> apiClient, IMap<WasteCodeDataAndNotificationData, CustomWasteCodesViewModel> mapper)
        {
            this.apiClient = apiClient;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            using (var client = apiClient())
            {
                var result =
                    await
                        client.SendAsync(User.GetAccessToken(),
                            new GetWasteCodeLookupAndNotificationDataByTypes(id, null, new[]
                            {
                                CodeType.ExportCode,
                                CodeType.ImportCode,
                                CodeType.CustomsCode,
                                CodeType.OtherCode
                            }));

                return View(mapper.Map(result));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, CustomWasteCodesViewModel model, bool backToOverview = false)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                try
                {
                    await
                        client.SendAsync(User.GetAccessToken(),
                            new SetCustomWasteCodes(id,
                                model.ExportNationalCode,
                                model.ExportNationalCodeNotApplicable,
                                model.ImportNationalCode,
                                model.ImportNationalCodeNotApplicable,
                                model.CustomsCode,
                                model.CustomsCodeNotApplicable,
                                model.OtherCode,
                                model.OtherCodeNotApplicable));

                    var notificationInfo =
                        await client.SendAsync(User.GetAccessToken(), new GetNotificationBasicInfo(id));

                    if (notificationInfo.NotificationType == NotificationType.Recovery)
                    {
                        return (backToOverview) ? RedirectToAction("Index", "Home", new { id })
                            : RedirectToAction("RecoveryPercentage", "RecoveryInfo", new { id });    
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { id });
                    }
                }
                catch (ApiBadRequestException ex)
                {
                    this.HandleBadRequest(ex);
                    if (ModelState.IsValid)
                    {
                        throw;
                    }
                }
                return View(model);
            }
        }
    }
}