namespace EA.Iws.Web.Infrastructure.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Notification;
    using Core.Shared;
    using Services;

    public class CompetentAuthorityEmailAddressAttribute : ValidationAttribute
    {
        private readonly string competentAuthorityProperty;
        private readonly string errorMessage = "Email address must end in {0}";

        public CompetentAuthorityEmailAddressAttribute(string competentAuthorityProperty)
        {
            this.competentAuthorityProperty = competentAuthorityProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var config = DependencyResolver.Current.GetService<AppConfiguration>();

            if (config.Environment.Equals("LIVE", StringComparison.InvariantCultureIgnoreCase))
            {
                var instance = validationContext.ObjectInstance;
                var type = instance.GetType();

                var competentAuthorityPropertyValue = type.GetProperty(competentAuthorityProperty)
                    .GetValue(instance, null);

                if (!typeof(CompetentAuthority).IsAssignableFrom(competentAuthorityPropertyValue.GetType()))
                {
                    throw new InvalidOperationException("The competentAuthorityProperty must be of type " +
                                                        typeof(CompetentAuthority).FullName);
                }

                var competentAuthority = (CompetentAuthority)competentAuthorityPropertyValue;

                var validEmailAddressDomains =
                    CompetentAuthorityMetadata.GetValidEmailAddressDomains(competentAuthority).ToArray();
                if (validEmailAddressDomains.Any(domain => value.ToString().EndsWith(domain)))
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult(string.Format(errorMessage, string.Join(" or ", validEmailAddressDomains)), new[] { validationContext.MemberName });
            }

            return ValidationResult.Success;
        }
    }
}