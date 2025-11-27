namespace EA.Iws.RequestHandlers.Mappings
{
    using EA.Iws.Core.MessageBanner;
    using EA.Iws.Domain;
    using EA.Prsd.Core.Mapper;

    internal class MessageBannerMap : IMap<MessageBanner, MessageBannerData>
    {
        public MessageBannerMap()
        {
        }

        public MessageBannerData Map(MessageBanner messageBanner)
        {
            return new MessageBannerData
            {
                Title = messageBanner.Title,
                Description = messageBanner.Description
            };
        }
    }
}
