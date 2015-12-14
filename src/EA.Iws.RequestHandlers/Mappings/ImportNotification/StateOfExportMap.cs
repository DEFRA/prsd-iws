namespace EA.Iws.RequestHandlers.Mappings.ImportNotification
{
    using Core.ImportNotification.Draft;
    using Prsd.Core.Mapper;

    internal class StateOfExportMap : IMap<StateOfExport, Domain.ImportNotification.StateOfExport>
    {
        public Domain.ImportNotification.StateOfExport Map(StateOfExport source)
        {
            return new Domain.ImportNotification.StateOfExport(source.ExitPointId.Value,
                source.CompetentAuthorityId.Value, source.CountryId.Value);
        }
    }
}