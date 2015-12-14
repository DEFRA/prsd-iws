namespace EA.Iws.RequestHandlers.Mappings.ImportNotification
{
    using Core.ImportNotification.Draft;
    using Prsd.Core.Mapper;

    internal class TransitStateMap : IMap<TransitState, Domain.ImportNotification.TransitState>
    {
        public Domain.ImportNotification.TransitState Map(TransitState source)
        {
            return new Domain.ImportNotification.TransitState(source.EntryPointId.Value, source.ExitPointId.Value,
                source.CountryId.Value, source.CompetentAuthorityId.Value, source.OrdinalPosition);
        }
    }
}