namespace EA.Iws.Domain
{
    using System;
    using Core.Shared;

    public class ValuePerWeight
    {
        private readonly bool canHaveNegativeAmount;

        public ValuePerWeightUnits Units { get; private set; }
        public decimal Amount { get; private set; }

        protected ValuePerWeight()
        {
        }

        protected ValuePerWeight(ValuePerWeightUnits units, decimal amount, bool canHaveNegativeAmount = false)
        {
            this.canHaveNegativeAmount = canHaveNegativeAmount;
            CheckIsValid(units, amount);
            Units = units;
            Amount = amount;
        }

        private void CheckIsValid(ValuePerWeightUnits units, decimal amount)
        {
            if (!canHaveNegativeAmount && amount < 0)
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