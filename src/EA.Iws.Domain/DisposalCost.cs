namespace EA.Iws.Domain
{
    using Core.Shared;

    public class DisposalCost
    {
        public ValuePerWeightUnits? Units { get; private set; }
        public decimal? Amount { get; private set; }

        internal DisposalCost()
        {
        }

        public DisposalCost(ValuePerWeightUnits? units, decimal? amount)
        {
            Units = units;
            Amount = amount;
        }
    }
}