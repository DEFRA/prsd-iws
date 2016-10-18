namespace EA.Iws.Web.Areas.Admin.ViewModels.ImportNotification
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Infrastructure.Validation;
    using Web.ViewModels.Shared;

    public class NotificationReceivedDateViewModel : IValidatableObject
    {
        public string NotificationNumber { get; set; }

        [Display(Name = "NotificationReceivedLabel", ResourceType = typeof(NotificationReceivedDateViewModelResources))]
        [RequiredDateInput(ErrorMessageResourceName = "NotificationReceivedRequired", ErrorMessageResourceType = typeof(NotificationReceivedDateViewModelResources))]
        public OptionalDateInputViewModel NotificationReceived { get; set; }

        public NotificationReceivedDateViewModel()
        {
            NotificationReceived = new OptionalDateInputViewModel(true);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NotificationReceived.AsDateTime() > DateTime.UtcNow)
            {
                yield return new ValidationResult(NotificationReceivedDateViewModelResources.ReceivedNotInFuture, new[] { "NotificationReceived" });
            }
        }
    }
}