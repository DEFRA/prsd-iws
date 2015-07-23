namespace EA.Iws.Web.Tests.Unit.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Areas.NotificationApplication.Controllers;
    using Core.Notification;
    using Core.Shared;
    using FakeItEasy;
    using Requests.Facilities;
    using Requests.Notification;
    using Requests.Shared;
    using Web.ViewModels.Shared;
    using Xunit;

    public class FacilityControllerTests
    {
        private readonly IIwsClient client;
        private readonly Guid notificationId = new Guid("4AB23CDF-9B24-4598-A302-A69EBB5F2152");
        private readonly Guid facilityId = new Guid("2196585B-F0F0-4A01-BC2F-EB8191B30FC6");
        private readonly FacilityController facilityController;
        private Guid facilityId2 = new Guid("D8991991-64A7-4101-A3A2-2F6B538A0A7A");

        public FacilityControllerTests()
        {
            client = A.Fake<IIwsClient>();

            A.CallTo(() => client.SendAsync(A<GetCountries>._)).Returns(new List<CountryData>
            {
                new CountryData
                {
                    Id = new Guid("4345FB05-F7DF-4E16-939C-C09FCA5C7D7B"),
                    Name = "United Kingdom"
                },
                new CountryData
                {
                    Id = new Guid("29B0D09E-BA77-49FB-AF95-4171408C07C9"),
                    Name = "Germany"
                }
            });

            A.CallTo(
                () =>
                    client.SendAsync(A<string>._,
                        A<GetNotificationBasicInfo>.That.Matches(p => p.NotificationId == notificationId)))
                .Returns(new NotificationBasicInfo
                {
                    CompetentAuthority = CompetentAuthority.England,
                    NotificationId = notificationId,
                    NotificationNumber = "GB 0001 002000",
                    NotificationType = NotificationType.Recovery
                });

            facilityController = new FacilityController(() => client);
        }

        [Fact]
        public async Task CopyFromImporter_ReturnsView()
        {
            var result = await facilityController.CopyFromImporter(notificationId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task CopyFromImporter_Post_InvalidModelReturnsView()
        {
            var model = new YesNoChoiceViewModel();

            facilityController.ModelState.AddModelError("Test", "Error");

            var result = await facilityController.CopyFromImporter(notificationId, model) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task CopyFromImporter_Post_YesCallsClient()
        {
            var model = new YesNoChoiceViewModel();
            model.Choices.SelectedValue = "Yes";
            await facilityController.CopyFromImporter(notificationId, model);

            A.CallTo(
                () =>
                    client.SendAsync(A<string>._,
                        A<CopyFacilityFromImporter>.That.Matches(p => p.NotificationId == notificationId)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task CopyFromImporter_Post_NoDoesntCallClient()
        {
            var model = new YesNoChoiceViewModel();
            model.Choices.SelectedValue = "No";
            await facilityController.CopyFromImporter(notificationId, model);

            A.CallTo(() => client.SendAsync(A<string>._, A<CopyFacilityFromImporter>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task CopyFromImporter_Post_ReturnsListView()
        {
            var model = new YesNoChoiceViewModel();
            model.Choices.SelectedValue = "No";
            var result = await facilityController.CopyFromImporter(notificationId, model) as RedirectToRouteResult;

            Assert.Equal("List", result.RouteValues["action"]);
        }

        [Fact]
        public async Task CopyFromImporter_NotificationTypeSetInViewBag()
        {
            var result = await facilityController.CopyFromImporter(notificationId) as ViewResult;

            Assert.Equal(NotificationType.Recovery.ToString().ToLowerInvariant(), result.ViewBag.NotificationType);
        }
    }
}