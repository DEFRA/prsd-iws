namespace EA.Iws.Web.Tests.Unit.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Areas.NotificationApplication.Controllers;
    using FakeItEasy;
    using Requests.Producers;
    using Web.ViewModels.Shared;
    using Xunit;

    public class ProducerControllerTests
    {
        private readonly IIwsClient client;
        private readonly Guid notificationId = new Guid("4AB23CDF-9B24-4598-A302-A69EBB5F2152");
        private readonly ProducerController producerController;

        public ProducerControllerTests()
        {
            client = A.Fake<IIwsClient>();
            producerController = new ProducerController(() => client);
        }

        [Fact]
        public void CopyFromExporter_ReturnsView()
        {
            var result = producerController.CopyFromExporter(notificationId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task CopyFromExporter_Post_InvalidModelReturnsView()
        {
            var model = new YesNoChoiceViewModel();

            producerController.ModelState.AddModelError("Test", "Error");

            var result = await producerController.CopyFromExporter(notificationId, model) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task CopyFromExporter_Post_YesCallsClient()
        {
            var model = new YesNoChoiceViewModel();
            model.Choices.SelectedValue = "Yes";
            await producerController.CopyFromExporter(notificationId, model);

            A.CallTo(
                () =>
                    client.SendAsync(A<string>._,
                        A<CopyProducerFromExporter>.That.Matches(p => p.NotificationId == notificationId)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task CopyFromExporter_Post_NoDoesntCallClient()
        {
            var model = new YesNoChoiceViewModel();
            model.Choices.SelectedValue = "No";
            await producerController.CopyFromExporter(notificationId, model);

            A.CallTo(() => client.SendAsync(A<string>._, A<CopyProducerFromExporter>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task CopyFromExporter_Post_ReturnsListView()
        {
            var model = new YesNoChoiceViewModel();
            model.Choices.SelectedValue = "No";
            var result = await producerController.CopyFromExporter(notificationId, model) as RedirectToRouteResult;

            Assert.Equal("List", result.RouteValues["action"]);
        }
    }
}