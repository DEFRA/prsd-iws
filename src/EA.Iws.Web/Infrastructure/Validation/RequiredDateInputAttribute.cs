namespace EA.Iws.Web.Infrastructure.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using ViewModels.Shared;

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class RequiredDateInputAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var inputDate = value as OptionalDateInputViewModel;

            if (inputDate != null && !inputDate.IsCompleted)
            {
                return new ValidationResult(ErrorMessageString, new[] { "Day" });
            }

            return ValidationResult.Success;
        }
    }
}