namespace EA.Iws.Web.Infrastructure.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class IsValidNumberAttribute : ValidationAttribute
    {
        private readonly int precision;

        public IsValidNumberAttribute(int maxPrecision)
        {
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
            var minNumber = 0 - maxNumber;

            return number > minNumber && number < maxNumber;
        }
    }
}