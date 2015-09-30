namespace EA.Iws.Domain
{
    using System;
    using Prsd.Core;

    public class Percentage
    {
        public decimal Value { get; private set; }

        protected Percentage()
        {
        }

        public Percentage(decimal value)
        {
            if (value > 100)
            {
                throw new ArgumentOutOfRangeException("value", "Cannot set a percentage greater than 100%");
            }

            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value", "Cannot set a negative percentage");
            }

            Value = decimal.Round(value, 2, MidpointRounding.AwayFromZero);
        }
    }
}
