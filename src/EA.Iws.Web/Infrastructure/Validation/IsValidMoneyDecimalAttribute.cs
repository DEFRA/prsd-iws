namespace EA.Iws.Web.Infrastructure.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IsValidMoneyDecimalAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var validationResult = new ValidationResult("Please enter a number with a maximum of 2 decimal places");

            if (value.ToString().IsValidMoneyDecimal())
            {
                validationResult = ValidationResult.Success;
            }

            return validationResult;
        }
    }
}