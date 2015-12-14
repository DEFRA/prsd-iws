namespace EA.Iws.RequestHandlers.Mappings.ImportNotification
{
    using Core.ImportNotification.Draft;
    using Prsd.Core.Mapper;

    internal class ExporterMap : IMap<Exporter, Domain.ImportNotification.Exporter>
    {
        private readonly IMapper mapper;

        public ExporterMap(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public Domain.ImportNotification.Exporter Map(Exporter source)
        {
            return new Domain.ImportNotification.Exporter(
                source.ImportNotificationId,
                source.BusinessName,
                mapper.Map<Domain.ImportNotification.Address>(source.Address),
                mapper.Map<Domain.ImportNotification.Contact>(source.Contact));
        }
    }
}