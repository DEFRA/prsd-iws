namespace EA.Iws.Domain
{
    using Core.Shared;

    public class RecoveryCost : ValuePerWeight
    {
        protected RecoveryCost()
        {
        }

        public RecoveryCost(ValuePerWeightUnits units, decimal amount)
            : base(units, amount)
        {
        }
    }
}
