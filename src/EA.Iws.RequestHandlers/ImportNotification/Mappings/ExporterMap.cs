namespace EA.Iws.RequestHandlers.ImportNotification.Mappings
{
    using Prsd.Core.Mapper;
    using Core = Core.ImportNotification.Summary;
    using Domain = Domain.ImportNotification;

    internal class ExporterMap : IMap<Domain.Exporter, Core.Exporter>
    {
        private readonly IMapper mapper;

        public ExporterMap(IMapper mapper)
        {
            this.mapper = mapper;
        }
        public Core.Exporter Map(Domain.Exporter source)
        {
            Core.Exporter result = null;

            if (source != null)
            {
                result = new Core.Exporter
                {
                    Name = source.Name,
                    BusinessType = source.Type,
                    RegistrationNumber = source.RegistrationNumber,
                    Address = mapper.Map<Core.Address>(source.Address),
                    Contact = mapper.Map<Core.Contact>(source.Contact)
                };
            }

            return result;
        }
    }
}