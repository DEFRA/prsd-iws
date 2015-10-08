namespace EA.Iws.Web.Infrastructure.Attributes
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IsValidMoneyDecimal : ValidationAttribute
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