namespace EA.Iws.RequestHandlers.ImportNotification.Mappings
{
    using Prsd.Core.Mapper;
    using Core = Core.ImportNotification.Summary;
    using Domain = Domain.ImportNotification;

    internal class ChemicalCompositionMap : IMap<Domain.WasteType, Core.ChemicalComposition>
    {
        public Core.ChemicalComposition Map(Domain.WasteType source)
        {
            return new Core.ChemicalComposition
            {
                Composition = source.ChemicalCompositionType
            };
        }
    }
}