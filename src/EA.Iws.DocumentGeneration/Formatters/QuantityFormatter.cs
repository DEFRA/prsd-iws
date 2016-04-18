namespace EA.Iws.DocumentGeneration.Formatters
{
    using Core.Shared;

    public class QuantityFormatter
    {
        private const string FormatString = "G29";

        public string QuantityToStringWithUnits(decimal? quantity, ShipmentQuantityUnits? units)
        {
            if (!quantity.HasValue || !units.HasValue)
            {
                return string.Empty;
            }

            switch (units.Value)
            {
                case ShipmentQuantityUnits.Kilograms:
                    return quantity.Value.ToString(FormatString) + " kg";
                case ShipmentQuantityUnits.Litres:
                    return quantity.Value.ToString(FormatString) + " Ltrs";
                default:
                    return quantity.Value.ToString(FormatString);
            }
        }
    }
}
