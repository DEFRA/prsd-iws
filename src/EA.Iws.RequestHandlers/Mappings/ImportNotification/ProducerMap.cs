namespace EA.Iws.RequestHandlers.Mappings.ImportNotification
{
    using Core.ImportNotification.Draft;
    using Prsd.Core.Mapper;

    internal class ProducerMap : IMap<Producer, Domain.ImportNotification.Producer>
    {
        private readonly IMapper mapper;

        public ProducerMap(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public Domain.ImportNotification.Producer Map(Producer source)
        {
            return new Domain.ImportNotification.Producer(
                source.ImportNotificationId,
                source.BusinessName,
                mapper.Map<Domain.ImportNotification.Address>(source.Address),
                mapper.Map<Domain.ImportNotification.Contact>(source.Contact),
                !source.AreMultiple);
        }
    }
}