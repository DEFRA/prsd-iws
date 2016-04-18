namespace EA.Iws.RequestHandlers.Mappings.ImportNotification
{
    using Core.ImportNotification.Draft;
    using Prsd.Core.Mapper;

    internal class ImporterMap : IMap<Importer, Domain.ImportNotification.Importer>
    {
        private readonly IMapper mapper;

        public ImporterMap(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public Domain.ImportNotification.Importer Map(Importer source)
        {
            return new Domain.ImportNotification.Importer(
                source.ImportNotificationId,
                source.BusinessName,
                source.Type.Value,
                source.RegistrationNumber,
                mapper.Map<Domain.ImportNotification.Address>(source.Address),
                mapper.Map<Domain.ImportNotification.Contact>(source.Contact));
        }
    }
}