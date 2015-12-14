namespace EA.Iws.RequestHandlers.Mappings.ImportNotification
{
    using System.Linq;
    using Core.ImportNotification.Draft;
    using Domain.ImportNotification;
    using Prsd.Core.Mapper;
    using Facility = Domain.ImportNotification.Facility;
    using FacilityCollection = Core.ImportNotification.Draft.FacilityCollection;

    internal class FacilityCollectionMap :
        IMapWithParameter<FacilityCollection, Preconsented, Domain.ImportNotification.FacilityCollection>
    {
        private readonly IMapper mapper;

        public FacilityCollectionMap(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public Domain.ImportNotification.FacilityCollection Map(FacilityCollection source, Preconsented parameter)
        {
            return new Domain.ImportNotification.FacilityCollection(parameter.ImportNotificationId,
                new FacilityList(source.Facilities.Select(f => mapper.Map<Facility>(f))),
                parameter.AllFacilitiesPreconsented.Value);
        }
    }
}