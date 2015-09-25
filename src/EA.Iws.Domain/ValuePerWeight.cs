namespace EA.Iws.Domain
{
    using Core.Shared;

    public class ValuePerWeight
    {
        public ValuePerWeightUnits Units { get; private set; }
        public decimal Amount { get; private set; }

        protected ValuePerWeight()
        {
        }

        public ValuePerWeight(ValuePerWeightUnits units, decimal amount)
        {
            Units = units;
            Amount = amount;
        }
    }
}