namespace EA.Iws.Domain
{
    using System;
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
            CheckIsValid(units, amount);
            Units = units;
            Amount = amount;
        }

        private void CheckIsValid(ValuePerWeightUnits units, decimal amount)
        {
            if (amount < 0)
            {
                throw new InvalidOperationException("The amount cannot be negative");
            }

            if (units == default(ValuePerWeightUnits))
            {
                throw new InvalidOperationException("Units cannot be the default value");
            }
        }
    }
}