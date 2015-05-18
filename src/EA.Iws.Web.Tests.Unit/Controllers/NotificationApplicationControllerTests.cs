namespace EA.Iws.Web.Tests.Unit.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using FakeItEasy;
    using Prsd.Core.Web.ApiClient;
    using Requests.Notification;
    using Requests.Shared;
    using ViewModels.NotificationApplication;
    using ViewModels.Shared;
    using Web.Controllers;
    using Xunit;

    public class NotificationApplicationControllerTests
    {
        private static CompetentAuthorityChoiceViewModel CompetentAuthorityChoice
        {
            get
            {
                var competentAuthorityChoice = new CompetentAuthorityChoiceViewModel
                {
                    CompetentAuthorities = new RadioButtonStringCollectionViewModel
                    {
                        SelectedValue = "Test",
                        PossibleValues = new[] { "Test", "String", "Value" }
                    }
                };
                return competentAuthorityChoice;
            }
        }

        private static NotificationApplicationController CreateNotificationApplicationController()
        {
            var client = A.Fake<IIwsClient>();
            A.CallTo(() => client.SendAsync(A<string>._, A<CreateNotificationApplication>._))
                .Returns(new ApiResponse<Guid>(HttpStatusCode.OK, Guid.Empty));
            return new NotificationApplicationController(() => client);
        }

        [Fact]
        public void CompetentAuthority_Get_ReturnsCorrectView()
        {
            var controller = CreateNotificationApplicationController();

            var result = controller.CompetentAuthority() as ViewResult;

            Assert.Equal("CompetentAuthority", result.ViewName);
            Assert.IsType<CompetentAuthorityChoiceViewModel>(result.Model);
        }

        [Fact]
        public void CompetentAuthority_Post_RedirectsToCorrectAction()
        {
            var controller = CreateNotificationApplicationController();

            var competentAuthorityChoice = CompetentAuthorityChoice;

            var result = controller.CompetentAuthority(competentAuthorityChoice) as RedirectToRouteResult;

            Assert.Equal("NotificationTypeQuestion", result.RouteValues["action"]);
        }

        [Fact]
        public void CompetentAuthority_PostInvalidModel_ReturnsToCorrectView()
        {
            var controller = CreateNotificationApplicationController();

            controller.ModelState.AddModelError("Error", "A Test Error");

            var competentAuthorityChoice = CompetentAuthorityChoice;

            var result = controller.CompetentAuthority(competentAuthorityChoice) as ViewResult;

            Assert.Equal("CompetentAuthority", result.ViewName);
            Assert.IsType<CompetentAuthorityChoiceViewModel>(result.Model);
        }

        [Fact]
        public void NotificationTypeQuestion_Get_ReturnsCorrectView()
        {
            var controller = CreateNotificationApplicationController();
            var result = controller.NotificationTypeQuestion("Environment Agency", "Recovery") as ViewResult;

            Assert.Empty(result.ViewName);
            Assert.IsType<InitialQuestionsViewModel>(result.Model);
        }

        [Fact]
        public async Task NotificationTypeQuestion_Post_RedirectsToCorrectAction()
        {
            var controller = CreateNotificationApplicationController();
            var model = new InitialQuestionsViewModel();
            model.SelectedNotificationType = NotificationType.Recovery;
            var result = await controller.NotificationTypeQuestion(model) as RedirectToRouteResult;

            Assert.Equal("Created", result.RouteValues["action"]);
        }

        [Fact]
        public async Task NotificationTypeQuestion_PostInvalidModel_ReturnsToCorrectView()
        {
            var controller = CreateNotificationApplicationController();
            controller.ModelState.AddModelError("Error", "Test Error");
            var model = new InitialQuestionsViewModel();
            var result = await controller.NotificationTypeQuestion(model) as ViewResult;

            Assert.Empty(result.ViewName);
            Assert.IsType<InitialQuestionsViewModel>(result.Model);
        }

        private static ExporterNotifier ExporterNotifierViewModel
        {
            get
            {
                ExporterNotifier exporter = new ExporterNotifier();

                exporter.BusinessViewModel = new BusinessViewModel()
                {
                    Name = "test exporter",
                    EntityType = "Sole Trader",
                    SoleTraderRegistrationNumber = "ST101",
                    AdditionalRegistrationNumber = "00001"
                };

                exporter.AddressDetails = new AddressViewModel()
                {
                    Address1 = "Address Line 1",
                    Address2 = "Address Line 2",
                    Building = "My Building",
                    County = "Surrey",
                    Postcode = "XY12 3AB",
                    TownOrCity = "Mycity"
                };

                exporter.ContactDetails = new ContactPersonViewModel()
                {
                    FirstName = "ContactFirstName",
                    LastName = "ContactLastName",
                    Telephone = "+441234567890",
                    Fax = "123",
                    Email = "test@test.com"
                };

                return exporter;
            }
        }

        [Fact]
        public async Task Exporter_Get_ReturnsCorrectView()
        {
            var controller = CreateNotificationApplicationController();

            var result = await controller.ExporterNotifier(new Guid()) as ViewResult;

            Assert.True(result != null);
            Assert.Equal(String.Empty, result.ViewName);
            Assert.IsType<ExporterNotifier>(result.Model);
        }

        [Fact]
        public async Task Exporter_Post_RedirectsToCorrectAction()
        {
            var controller = CreateNotificationApplicationController();

            var exporter = ExporterNotifierViewModel;

            var result = await controller.ExporterNotifier(exporter) as RedirectToRouteResult;

            Assert.True(result != null);
            Assert.Equal("CopyFromExporter", result.RouteValues["action"]);
        }

        [Fact]
        public async Task Exporter_Post_InvalidModel_ReturnsToCorrectView()
        {
            var controller = CreateNotificationApplicationController();

            controller.ModelState.AddModelError("Error", "A Test Error");
            var result = await controller.ExporterNotifier(new Guid()) as ViewResult;

            Assert.Equal(String.Empty, result.ViewName);
            Assert.IsType<ExporterNotifier>(result.Model);
        }
    }
}