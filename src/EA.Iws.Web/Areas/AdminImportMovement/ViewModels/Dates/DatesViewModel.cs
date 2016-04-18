namespace EA.Iws.Web.Areas.AdminImportMovement.ViewModels.Dates
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Core.ImportMovement;
    using Infrastructure.Validation;
    using Web.ViewModels.Shared;

    public class DatesViewModel
    {
        [Display(Name = "ActualShipmentDate", ResourceType = typeof(DatesViewModelResources))]
        [RequiredDateInput(ErrorMessageResourceName = "ActualShipmentDateRequired", ErrorMessageResourceType = typeof(DatesViewModelResources))]
        public OptionalDateInputViewModel ActualShipmentDate { get; set; }

        [Display(Name = "PrenotificationDate", ResourceType = typeof(DatesViewModelResources))]
        public OptionalDateInputViewModel PrenotificationDate { get; set; }

        public int Number { get; set; }

        public Guid NotificationId { get; set; }

        public DatesViewModel()
        {
            ActualShipmentDate = new OptionalDateInputViewModel(true);
            PrenotificationDate = new OptionalDateInputViewModel(true);
        }

        public DatesViewModel(ImportMovementData data)
        {
            NotificationId = data.NotificationId;

            ActualShipmentDate = new OptionalDateInputViewModel(data.ActualDate.DateTime, true);

            var prenotificationDate = data.PreNotificationDate.HasValue
                ? data.PreNotificationDate.Value.DateTime : (DateTime?)null;

            PrenotificationDate = new OptionalDateInputViewModel(prenotificationDate, true);

            Number = data.Number;
        }
    }
}