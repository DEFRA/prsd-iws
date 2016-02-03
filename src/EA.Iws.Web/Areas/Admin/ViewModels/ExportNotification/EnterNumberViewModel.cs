namespace EA.Iws.Web.Areas.Admin.ViewModels.ExportNotification
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.Notification;
    using Core.Shared;

    public class EnterNumberViewModel : IValidatableObject
    {
        public CompetentAuthority CompetentAuthority { get; set; }

        public NotificationType NotificationType { get; set; }

        [Display(Name = "Notification number")]
        [Required(ErrorMessage = "Please enter the notification number")]
        public int? Number { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Number <= 0)
            {
                yield return new ValidationResult("The notification number must be greater than or equal to 1", new[] { "Number" });
            }

            if (CompetentAuthority == CompetentAuthority.England && Number >= 5000)
            {
                yield return new ValidationResult("The notification number must be less than 5000", new[] { "Number" });
            }
            else if (CompetentAuthority == CompetentAuthority.Scotland && Number >= 500)
            {
                yield return new ValidationResult("The notification number must be less than 500", new[] { "Number" });
            }
            else if (CompetentAuthority == CompetentAuthority.NorthernIreland && Number >= 1000)
            {
                yield return new ValidationResult("The notification number must be less than 1000", new[] { "Number" });
            }
            else if (CompetentAuthority == CompetentAuthority.Wales && Number >= 100)
            {
                yield return new ValidationResult("The notification number must be less than 100", new[] { "Number" });
            }
        }
    }
}