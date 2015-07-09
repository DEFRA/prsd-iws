namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.Notification;
    using Infrastructure;
    using Prsd.Core.Mapper;
    using Requests.Notification;
    using Requests.Submit;
    using ViewModels.NotificationApplication;
    using ViewModels.Submit;

    public class SubmitController : Controller
    {
        private readonly Func<IIwsClient> apiClient;
        private readonly IMapWithParentObjectId<SubmitSummaryData, SubmitSideBarViewModel> mapper;

        public SubmitController(Func<IIwsClient> apiClient, IMapWithParentObjectId<SubmitSummaryData, SubmitSideBarViewModel> mapper)
        {
            this.apiClient = apiClient;
            this.mapper = mapper;
        }

        public ActionResult SubmitSideBar(Guid id, int charge)
        {
            using (var client = apiClient())
            {
                var result = client.SendAsync(User.GetAccessToken(),
                    new GetSubmitSummaryInformationForNotification(id)).GetAwaiter().GetResult();

                result.Charge = charge;

                return PartialView("_SubmitSideBar", mapper.Map(result, id));
            }
        }

        public async Task<ActionResult> Index(Guid id)
        {
            using (var client = apiClient())
            {
                var result = await client.SendAsync(User.GetAccessToken(),
                    new GetNotificationInfo(id));

                var model = new NotificationOverviewViewModel(result);
                ViewBag.Charge = result.NotificationCharge;
                
                return View(model);
            }
        }
    }
}