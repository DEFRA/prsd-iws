namespace EA.Iws.RequestHandlers.Mappings.ImportNotification
{
    using Core.ImportNotification.Draft;
    using Prsd.Core.Mapper;

    internal class StateOfImportMap : IMap<StateOfImport, Domain.ImportNotification.StateOfImport>
    {
        public Domain.ImportNotification.StateOfImport Map(StateOfImport source)
        {
            return new Domain.ImportNotification.StateOfImport(source.EntryPointId.Value,
                source.CompetentAuthorityId.Value);
        }
    }
}