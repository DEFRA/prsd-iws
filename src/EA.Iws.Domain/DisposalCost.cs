namespace EA.Iws.Domain
{
    using Core.Shared;

    public class DisposalCost : ValuePerWeight
    {
        internal DisposalCost()
        {
        }

        public DisposalCost(ValuePerWeightUnits units, decimal amount)
            : base(units, amount)
        {
        }
    }
}