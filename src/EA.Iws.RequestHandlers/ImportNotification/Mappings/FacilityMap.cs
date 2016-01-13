namespace EA.Iws.RequestHandlers.ImportNotification.Mappings
{
    using Prsd.Core.Mapper;
    using Core = Core.ImportNotification.Summary;
    using Domain = Domain.ImportNotification;

    internal class FacilityMap : IMap<Domain.Facility, Core.Facility>
    {
        private readonly IMapper mapper;

        public FacilityMap(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public Core.Facility Map(Domain.Facility source)
        {
            return new Core.Facility
            {
                IsActualSite = source.IsActualSiteOfTreatment,
                Name = source.Name,
                RegistrationNumber = source.RegistrationNumber,
                BusinessType = source.Type,
                Address = mapper.Map<Core.Address>(source.Address),
                Contact = mapper.Map<Core.Contact>(source.Contact)
            };
        }
    }
}