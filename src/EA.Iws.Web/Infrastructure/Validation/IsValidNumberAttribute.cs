namespace EA.Iws.Web.Infrastructure.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class IsValidNumberAttribute : ValidationAttribute
    {
        private readonly int precision;
        private readonly bool allowNegative;

        public IsValidNumberAttribute(int maxPrecision, bool allowNegative)
        {
            this.allowNegative = allowNegative;
            this.precision = maxPrecision;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = new ValidationResult("Please enter a valid number");

            decimal number;
            if (decimal.TryParse(value.ToString(), out number))
            {
                if (NumberIsValid(number))
                {
                    validationResult = ValidationResult.Success;
                }
            }

            return validationResult;
        }

        private bool NumberIsValid(decimal number)
        {
            var maxNumber = (long)Math.Pow(10, precision) - 1;
            var minNumber = allowNegative ? 0 - maxNumber : 0;

            return number > minNumber && number < maxNumber;
        }
    }
}