namespace EA.Iws.Web.Infrastructure.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IsValidMoneyDecimalAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var validationResult = new ValidationResult("Please enter a valid amount");

            if (value.ToString().IsValidMoneyDecimal())
            {
                validationResult = ValidationResult.Success;
            }

            return validationResult;
        }
    }
}