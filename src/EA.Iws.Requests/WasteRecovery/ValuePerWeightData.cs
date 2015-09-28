namespace EA.Iws.Requests.WasteRecovery
{
    using Core.Shared;

    public class ValuePerWeightData
    {
        public decimal Amount { get; private set; }
        public ValuePerWeightUnits Unit { get; private set; }

        public ValuePerWeightData(decimal amount, ValuePerWeightUnits unit)
        {
            Unit = unit;
            Amount = amount;
        }
    }
}
