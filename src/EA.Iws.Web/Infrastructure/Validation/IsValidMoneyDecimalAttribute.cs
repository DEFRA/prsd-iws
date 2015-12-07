namespace EA.Iws.Web.Infrastructure.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [AttributeUsage(AttributeTargets.Property)]
    public class IsValidMoneyDecimalAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var validationResult = new ValidationResult("Please enter a number with a maximum of 2 decimal places");
            
            if (value != null && value.ToString().IsValidMoneyDecimal())
            {
                validationResult = ValidationResult.Success;
            }

            return validationResult;
        }
    }
}