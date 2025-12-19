namespace EA.Iws.Web.Tests.Unit.Controllers
{
    using EA.Iws.Core.MessageBanner;
    using EA.Iws.Requests.MessageBanner;
    using EA.Iws.Web.Controllers;
    using EA.Iws.Web.ViewModels.Shared;
    using EA.Prsd.Core.Mediator;
    using FakeItEasy;
    using FluentAssertions;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Xunit;

    public class BannerControllerTests
    {
        private readonly IMediator mediator;
        private readonly BannerController bannerController;

        public BannerControllerTests()
        {
            mediator = A.Fake<IMediator>();
            bannerController = new BannerController(mediator);
        }

        [Fact]
        public async Task MessageBanner_IsActive()
        {
            //Arrange
            var messageBannerData = new MessageBannerData()
            {
                Description = "Test Description",
                Title = "Test Tile",
                StartTime = System.DateTime.Now,
                EndTime = System.DateTime.Now.AddDays(2),
            };

            var messageBannerViewModel = new MessageBannerViewModel()
            {
                Description = messageBannerData.Description,
                Title = messageBannerData.Title,
                IsActive = true
            };

            // Act
            A.CallTo(() => mediator.SendAsync(A<GetMessageBanner>._)).Returns(messageBannerData);
            var result = await bannerController.MessageBannerAsync() as JsonResult;

            // Assert
            result.Should().BeOfType<JsonResult>();
            result.JsonRequestBehavior.Should().Be(JsonRequestBehavior.AllowGet);
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(messageBannerViewModel);
        }

        [Fact]
        public async Task MessageBanner_IsInActive()
        {
            //Arrange
            MessageBannerData messageBannerData = null;
            var messageBannerViewModel = new MessageBannerViewModel()
            {
                Description = null,
                Title = null,
                IsActive = false
            };

            // Act
            A.CallTo(() => mediator.SendAsync(A<GetMessageBanner>._)).Returns(messageBannerData);
            var result = await bannerController.MessageBannerAsync() as JsonResult;

            // Assert
            result.Should().BeOfType<JsonResult>();
            result.JsonRequestBehavior.Should().Be(JsonRequestBehavior.AllowGet);
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.Should().BeEquivalentTo(messageBannerViewModel);
        }
    }
}
