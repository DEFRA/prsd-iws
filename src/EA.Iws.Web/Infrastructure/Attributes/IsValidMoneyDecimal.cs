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

            if (!string.IsNullOrWhiteSpace(value.ToString()))
            {
                Regex rgx = new Regex(@"^(?=[\d.])\d{0,3}(?:\d*|(?:,\d{3})*)(?:\.\d{1,2})?$");

                if (rgx.IsMatch(value.ToString()))
                {
                    validationResult = ValidationResult.Success;
                }
            }

            return validationResult;
        }
    }
}