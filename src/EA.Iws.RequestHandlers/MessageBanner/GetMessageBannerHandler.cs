namespace EA.Iws.RequestHandlers.MessageBanner
{
    using EA.Iws.Core.MessageBanner;
    using EA.Iws.Domain;
    using EA.Iws.Requests.MessageBanner;
    using EA.Prsd.Core.Mapper;
    using EA.Prsd.Core.Mediator;
    using System.Threading.Tasks;

    internal class GetMessageBannerHandler : IRequestHandler<GetMessageBanner, MessageBannerData>
    {
        private readonly IMessageBannerRepository messageBannerRepository;
        private readonly IMap<MessageBanner, MessageBannerData> mapper;

        public GetMessageBannerHandler(IMessageBannerRepository messageBannerRepository, IMap<MessageBanner, MessageBannerData> mapper)
        {
            this.messageBannerRepository = messageBannerRepository;
            this.mapper = mapper;
        }

        public async Task<MessageBannerData> HandleAsync(GetMessageBanner getMessageBanner)
        {
            var messageBanner = await messageBannerRepository.GetMessageBannerData();

            if (messageBanner == null)
            {
                return null;
            }
            return mapper.Map(messageBanner);
        }
    }
}
