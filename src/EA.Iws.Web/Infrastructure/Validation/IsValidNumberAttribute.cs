namespace EA.Iws.Web.Infrastructure.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IsValidNumberAttribute : ValidationAttribute
    {
        private readonly int precision;

        public bool IsOptional { get; set; }

        public NumberStyles NumberStyle { get; set; }

        public IsValidNumberAttribute(int maxPrecision)
        {
            this.precision = maxPrecision;
            NumberStyle = NumberStyles.Number;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var validationResult = new ValidationResult(ErrorMessageString ?? "Please enter a valid number");

            if (value != null)
            {
                decimal number;
                if (decimal.TryParse(value.ToString(), NumberStyle, new NumberFormatInfo(), out number))
                {
                    if (NumberIsValid(number))
                    {
                        validationResult = ValidationResult.Success;
                    }
                }
            }
            else if (IsOptional)
            {
                validationResult = ValidationResult.Success;
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