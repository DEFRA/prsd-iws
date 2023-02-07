namespace EA.Iws.Web.Tests.Unit.Controllers.Admin
{
    using EA.Iws.Requests.Notification;
    using EA.Iws.Web.Areas.Admin.Controllers;
    using EA.Iws.Web.Areas.Admin.ViewModels.ArchiveNotification;
    using EA.Prsd.Core.Mediator;
    using FakeItEasy;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Xunit;

    public class ArchiveNotificationControllerTests
    {
        private readonly ArchiveNotificationController controller;
        private readonly IMediator mediator;
        private readonly HttpContextBase context;
        private List<NotificationArchiveSummaryData> sampleNotifications;

        public ArchiveNotificationControllerTests()
        {
            mediator = A.Fake<IMediator>();
            controller = new ArchiveNotificationController(mediator);

            context = A.Fake<HttpContextBase>();
            context.Session["SelectedNotifications"] = JsonConvert.SerializeObject(new List<NotificationArchiveSummaryData>());

            controller.ControllerContext = new ControllerContext(context, new RouteData(), controller);

            sampleNotifications = new List<NotificationArchiveSummaryData>
            {
                new NotificationArchiveSummaryData
                {
                    Id = Guid.Parse("f64a5181-dee2-4e2c-bf4d-db4da2202da4"),
                    CompanyName = "Test",
                    IsArchived = false,
                    NotificationNumber = "UnitTest1",
                    PageNumber = 1,
                    CreatedDate = "01/01/2019"
                },
                new NotificationArchiveSummaryData
                {
                    Id = Guid.Parse("c150c236-574a-4e8b-960e-bc988049bed5"),
                    CompanyName = "Test 2",
                    IsArchived = false,
                    NotificationNumber = "UnitTest2",
                    PageNumber = 1,
                    CreatedDate = "01/01/2020"
                }
            };
        }

        [Fact]
        public async Task GetIndex_ReturnsNewViewModel()
        {
            var result = await controller.Index() as ViewResult;

            Assert.IsType<ArchiveNotificationResultViewModel>(result.Model);

            var model = result.Model as ArchiveNotificationResultViewModel;

            Assert.False(model.NumberOfNotificationsSelected > 0);
        }

        [Fact]
        public void PostIndex_SelectSingleNotification()
        {
            var selectedRecordList = new List<NotificationArchiveSummaryData> { sampleNotifications[0] };
            controller.SelectSingleNotification(selectedRecordList, true);

            Assert.True(HowManyNotificationsSelectedInHttpSession() == 1);
        }

        [Fact]
        public void PostIndex_SelectAllNotifications()
        {
            controller.SelectAllNotifications(sampleNotifications, true);
            Assert.True(HowManyNotificationsSelectedInHttpSession() == 2);
        }

        [Fact]
        public void PostIndex_Remove()
        {
            controller.SelectAllNotifications(sampleNotifications, true);
            Assert.True(HowManyNotificationsSelectedInHttpSession() == 2);

            controller.Remove(sampleNotifications[0].Id);
            Assert.True(HowManyNotificationsSelectedInHttpSession() == 1);
        }

        [Fact]
        public void PostIndex()
        {
            controller.SelectAllNotifications(sampleNotifications, true);

            var res = controller.Index(new ArchiveNotificationResultViewModel
            {
                IsSelectAllChecked = true,
                Notifications = sampleNotifications,
                NumberOfNotifications = 2,
                NumberOfNotificationsSelected = 2,
                PageNumber = 1,
                PageSize = 1,
                SelectedNotifications = sampleNotifications
            }) as ViewResult;

            Assert.True(res.ViewName == "Review");
        }

        [Fact]
        public void GetReview_ReturnsActionResult()
        {
            controller.SelectAllNotifications(sampleNotifications, true);
            var selectNotificationList = JsonConvert.DeserializeObject<List<NotificationArchiveSummaryData>>
                    (context.Session["SelectedNotifications"].ToString());

            var reviewModel = new ArchiveNotificationReviewViewModel()
            {
                SelectedNotifications = selectNotificationList,
                HasAnyResults = true
            };

            var result = controller.Review(reviewModel) as ViewResult;
            var model = result.Model as ArchiveNotificationReviewViewModel;

            Assert.True(model.HasAnyResults);
        }

        [Fact]
        public async void Post_Archive()
        {
            controller.SelectAllNotifications(sampleNotifications, true);
            var result = await controller.Archive() as ViewResult;
            var model = result.Model as ArchiveNotificationArchivedViewModel;

            Assert.True(result.ViewName == "Archived");
        }

        private int HowManyNotificationsSelectedInHttpSession()
        {
            var selectNotificationList = new List<NotificationArchiveSummaryData>();
            if (context.Session["SelectedNotifications"] != null)
            {
                selectNotificationList = JsonConvert.DeserializeObject<List<NotificationArchiveSummaryData>>
                    (context.Session["SelectedNotifications"].ToString());
            }

            return selectNotificationList.Count;
        }
    }
}