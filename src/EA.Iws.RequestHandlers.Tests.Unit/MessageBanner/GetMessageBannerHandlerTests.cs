namespace EA.Iws.RequestHandlers.Tests.Unit.MessageBanner
{
    using EA.Iws.Core.MessageBanner;
    using EA.Iws.Domain;
    using EA.Iws.RequestHandlers.MessageBanner;
    using EA.Iws.Requests.MessageBanner;
    using EA.Prsd.Core.Mapper;
    using FakeItEasy;
    using System.Threading.Tasks;
    using Xunit;

    public class GetMessageBannerHandlerTests
    {
        private readonly GetMessageBannerHandler handler;
        private readonly IMap<MessageBanner, MessageBannerData> mapper;
        private readonly IMessageBannerRepository repository;
        private readonly GetMessageBanner getMessageBanner;

        public GetMessageBannerHandlerTests()
        {
            repository = A.Fake<IMessageBannerRepository>();
            mapper = A.Fake<IMap<MessageBanner, MessageBannerData>>();
            handler = new GetMessageBannerHandler(repository, mapper);
            getMessageBanner = A.Fake<GetMessageBanner>();
        }

        [Fact]
        public async Task GetMessageBanners_ReturnsData()
        {
            var messageBanner = new MessageBanner("Test Title", "Test Description");
            var result = await handler.HandleAsync(getMessageBanner);

            A.CallTo(() => repository.GetMessageBannerData()).Returns(messageBanner);
            A.CallTo(() => mapper.Map(messageBanner));

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetMessageBanners_MustHaveHappenedOnceExactly()
        {
            var result = await handler.HandleAsync(getMessageBanner);

            A.CallTo(() => repository.GetMessageBannerData()).MustHaveHappenedOnceExactly();
        }
    }
}
