namespace EA.Iws.Web.ViewModels.Registration
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.Extensions;
    using Views.Registration;

    public abstract class CreateUserModelBase : IValidatableObject
    {
        public abstract string Password
        {
            get;
            set;
        }

        public abstract string Email
        {
            get;
            set;
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            var instance = validationContext.ObjectInstance;

            var password = instance.GetPropertyValue<string>("Password");
            var email = instance.GetPropertyValue<string>("Email");

            if (!string.IsNullOrWhiteSpace(password) && !string.IsNullOrWhiteSpace(email))
            {
                if (password.Equals(email))
                {
                    validationResults.Add(
                        new ValidationResult(ApplicantRegistrationResources.PasswordEmailMatch, new List<string> { "Password" }));
                }
            }

            return validationResults;
        }
    }
}