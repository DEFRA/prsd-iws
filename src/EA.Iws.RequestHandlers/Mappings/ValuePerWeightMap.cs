namespace EA.Iws.RequestHandlers.Mappings
{
    using Domain;
    using Prsd.Core.Mapper;
    using Requests.WasteRecovery;

    internal class ValuePerWeightMap : IMap<ValuePerWeight, ValuePerWeightData>
    {
        public ValuePerWeightData Map(ValuePerWeight source)
        {
            ValuePerWeightData data = null;

            if (source != null)
            {
                data = new ValuePerWeightData(source.Amount, source.Units);
            }

            return data;
        }
    }
}
