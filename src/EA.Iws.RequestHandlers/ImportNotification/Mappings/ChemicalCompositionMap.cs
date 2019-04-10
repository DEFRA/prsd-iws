namespace EA.Iws.RequestHandlers.ImportNotification.Mappings
{
    using Prsd.Core.Mapper;
    using Core = Core.ImportNotification.Summary;
    using Domain = Domain.ImportNotification;

    internal class ChemicalCompositionMap : IMap<Domain.WasteType, Core.ChemicalComposition>
    {
        public Core.ChemicalComposition Map(Domain.WasteType source)
        {
            Core.ChemicalComposition result = null;

            if (source != null)
            {
                result = new Core.ChemicalComposition
                {
                    Composition = source.ChemicalCompositionType
                };
            }

            return result;
        }
    }
}