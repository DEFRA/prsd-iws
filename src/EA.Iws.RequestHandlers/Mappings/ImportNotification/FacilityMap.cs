namespace EA.Iws.RequestHandlers.Mappings.ImportNotification
{
    using Core.ImportNotification.Draft;
    using Prsd.Core.Mapper;

    internal class FacilityMap : IMap<Facility, Domain.ImportNotification.Facility>
    {
        private readonly IMapper mapper;

        public FacilityMap(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public Domain.ImportNotification.Facility Map(Facility source)
        {
            return new Domain.ImportNotification.Facility(source.BusinessName, source.Type.Value,
                source.RegistrationNumber,
                mapper.Map<Domain.ImportNotification.Address>(source.Address),
                mapper.Map<Domain.ImportNotification.Contact>(source.Contact),
                source.IsActualSite);
        }
    }
}