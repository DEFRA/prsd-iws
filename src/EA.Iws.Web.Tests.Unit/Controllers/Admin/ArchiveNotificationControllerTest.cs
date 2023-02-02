namespace EA.Iws.Web.Tests.Unit.Controllers.Admin
{
    using EA.Iws.Requests.Notification;
    using EA.Iws.Web.Areas.Admin.Controllers;
    using EA.Iws.Web.Areas.Admin.ViewModels.ArchiveNotification;
    using EA.Iws.Web.Areas.Admin.ViewModels.Home;
    using EA.Prsd.Core.Mediator;
    using FakeItEasy;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.UI;
    using Xunit;

    public class ArchiveNotificationControllerTest
    {
        private readonly ArchiveNotificationController controller;
        private readonly IMediator mediator;
        private readonly ArchiveNotificationResultViewModel searchResultViewModel;

        public ArchiveNotificationControllerTest()
        {
            mediator = A.Fake<IMediator>();
            controller = new ArchiveNotificationController(mediator);
        }

        [Fact]
        public async Task GetIndex_ReturnsNewViewModel()
        {
            var context = A.Fake<HttpContextBase>();
            context.Session["SelectedNotifications"] = JsonConvert.SerializeObject(new List<NotificationArchiveSummaryData>());
            controller.ControllerContext = new ControllerContext(context, new RouteData(), controller);

            var result = await controller.Index() as ViewResult;

            Assert.IsType<ArchiveNotificationResultViewModel>(result.Model);

            var model = result.Model as ArchiveNotificationResultViewModel;

            Assert.False(model.NumberOfNotificationsSelected > 0);
        }

        public JsonResult PostIndex_SelectSingleNotification()
        {
            throw new NotImplementedException();
        }

        public JsonResult PostIndex_SelectAllNotifications()
        {
            throw new NotImplementedException();
        }

        public ActionResult PostIndex_Remove(Guid notificationId)
        {
            throw new NotImplementedException();
        }

        public ActionResult PostIndex()
        {
            throw new NotImplementedException();
        }

        public ActionResult GetReview_ReturnsActionResult()
        {
            throw new NotImplementedException();
        }
        public async Task<ActionResult> Post_Archive()
        {
            throw new NotImplementedException();
        }
    }
}