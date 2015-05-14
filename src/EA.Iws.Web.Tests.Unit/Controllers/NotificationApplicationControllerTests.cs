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

            Assert.Equal("WasteActionQuestion", result.RouteValues["action"]);
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
        public void WasteActionQuestion_Get_ReturnsCorrectView()
        {
            var controller = CreateNotificationApplicationController();
            var result = controller.WasteActionQuestion("Environment Agency", "Recovery") as ViewResult;

            Assert.Empty(result.ViewName);
            Assert.IsType<InitialQuestionsViewModel>(result.Model);
        }

        [Fact]
        public async Task WasteActionQuestion_Post_RedirectsToCorrectAction()
        {
            var controller = CreateNotificationApplicationController();
            var model = new InitialQuestionsViewModel();
            model.SelectedWasteAction = WasteAction.Recovery;
            var result = await controller.WasteActionQuestion(model) as RedirectToRouteResult;

            Assert.Equal("Created", result.RouteValues["action"]);
        }

        [Fact]
        public async Task WasteActionQuestion_PostInvalidModel_ReturnsToCorrectView()
        {
            var controller = CreateNotificationApplicationController();
            controller.ModelState.AddModelError("Error", "Test Error");
            var model = new InitialQuestionsViewModel();
            var result = await controller.WasteActionQuestion(model) as ViewResult;

            Assert.Empty(result.ViewName);
            Assert.IsType<InitialQuestionsViewModel>(result.Model);
        }

        private static ExporterNotifier ExporterNotifierViewModel
        {
            get
            {
                ExporterNotifier exporter = new ExporterNotifier();
                exporter.Name = "test exporter";
                exporter.EntityType = "Sole Trader";
                exporter.SoleTraderRegistrationNumber = "ST101";
                exporter.RegistrationNumber2 = "00001";

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
            Assert.Equal("ProducerInformation", result.RouteValues["action"]);
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