namespace EA.Iws.Web.Infrastructure.Validation
{
    using System.Collections;
    using System.ComponentModel.DataAnnotations;

    public class NotificationShareUserValidationAttribute : ValidationAttribute
    {
        private readonly int maximumEmails;

        public NotificationShareUserValidationAttribute(int max)
        {
            maximumEmails = max;
        }

        public override bool IsValid(object value)
        {
            var list = value as IList;

            if (list.Count >= maximumEmails)
            {
                return false;
            }

            return true;
        }
    }
}