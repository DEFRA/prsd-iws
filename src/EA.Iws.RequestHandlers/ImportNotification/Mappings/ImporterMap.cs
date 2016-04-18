namespace EA.Iws.RequestHandlers.ImportNotification.Mappings
{
    using Prsd.Core.Mapper;
    using Core = Core.ImportNotification.Summary;
    using Domain = Domain.ImportNotification;

    internal class ImporterMap : IMap<Domain.Importer, Core.Importer>
    {
        private readonly IMapper mapper;

        public ImporterMap(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public Core.Importer Map(Domain.Importer source)
        {
            return new Core.Importer
            {
                Name = source.Name,
                BusinessType = source.Type,
                RegistrationNumber = source.RegistrationNumber,
                Address = mapper.Map<Core.Address>(source.Address),
                Contact = mapper.Map<Core.Contact>(source.Contact)
            };
        }
    }
}