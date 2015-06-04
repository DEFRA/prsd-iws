namespace EA.Prsd.Core.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class RequiredIfPropertiesEqualAttribute : ValidationAttribute
    {
        private String DependantPropertyName { get; set; }
        private new String ErrorMessage { get; set; }
        private String PropertyName { get; set; }

        public RequiredIfPropertiesEqualAttribute(String dependentPropertyName, String propertyName, String errorMessage)
        {
            DependantPropertyName = dependentPropertyName;
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var instance = context.ObjectInstance;
            var type = instance.GetType();

            var dependantPropertyValue = type.GetProperty(DependantPropertyName).GetValue(instance, null);
            var propertyValue = type.GetProperty(PropertyName).GetValue(instance, null);
            var contextPropertyValue = type.GetProperty(context.MemberName).GetValue(instance, null);

            if (dependantPropertyValue != null && (dependantPropertyValue.ToString().Equals(propertyValue.ToString())))
            {
                if (contextPropertyValue == null || string.IsNullOrEmpty(contextPropertyValue.ToString()))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}