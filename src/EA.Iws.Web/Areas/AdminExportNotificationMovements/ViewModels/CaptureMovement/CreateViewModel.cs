namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.CaptureMovement
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Web.ViewModels.Shared;

    public class CreateViewModel : IValidatableObject
    {
        public int Number { get; set; }

        public Guid NotificationId { get; set; }

        [Display(Name = "PrenotificationDateLabel", ResourceType = typeof(CreateViewModelResources))]
        public OptionalDateInputViewModel PrenotificationDate { get; set; }

        [Display(Name = "ActualDateLabel", ResourceType = typeof(CreateViewModelResources))]
        public OptionalDateInputViewModel ActualShipmentDate { get; set; }

        [Display(Name = "HasNoPrenotification", ResourceType = typeof(CreateViewModelResources))]
        public bool HasNoPrenotification { get; set; }

        public CreateViewModel()
        {
            PrenotificationDate = new OptionalDateInputViewModel(true);
            ActualShipmentDate = new OptionalDateInputViewModel(true);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ActualShipmentDate.IsCompleted)
            {
                yield return new ValidationResult(CreateViewModelResources.ActualDateRequired, new[] { "ActualShipmentDate" });
            }

            if (!HasNoPrenotification && !PrenotificationDate.IsCompleted)
            {
                yield return new ValidationResult(CreateViewModelResources.PrenotificationDateRequired, 
                    new[] { "PrenotificationDate" });
            }
        }
    }
}