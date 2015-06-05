namespace EA.Iws.RequestHandlers.Mappings
{
    using Domain.TransportRoute;
    using Prsd.Core.Mapper;
    using Requests.TransportRoute;

    internal class EntryOrExitPointMap : IMap<EntryOrExitPoint, EntryOrExitPointData>
    {
        public EntryOrExitPointData Map(EntryOrExitPoint source)
        {
            return new EntryOrExitPointData
            {
                Id = source.Id,
                CountryId = source.Country.Id,
                Name = source.Name
            };
        }
    }
}
