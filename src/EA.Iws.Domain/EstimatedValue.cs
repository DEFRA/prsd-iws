namespace EA.Iws.Domain
{
    using Core.Shared;

    public class EstimatedValue : ValuePerWeight
    {
        protected EstimatedValue()
        {
        }

        public EstimatedValue(ValuePerWeightUnits units, decimal amount)
            : base(units, amount)
        {
        }
    }
}
