namespace EA.Iws.Web.Areas.AdminImportMovement.ViewModels.Dates
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Core.ImportMovement;
    using Web.ViewModels.Shared;

    public class DatesViewModel
    {
        [Display(Name = "ActualDate", ResourceType = typeof(DatesViewModelResources))]
        public OptionalDateInputViewModel ActualShipmentDate { get; set; }

        [Display(Name = "PrenotificationDate", ResourceType = typeof(DatesViewModelResources))]
        public OptionalDateInputViewModel PrenotificationDate { get; set; }

        public int Number { get; set; }

        public DatesViewModel()
        {
            ActualShipmentDate = new OptionalDateInputViewModel(true);
            PrenotificationDate = new OptionalDateInputViewModel(true);
        }

        public DatesViewModel(ImportMovementDates dates)
        {
            ActualShipmentDate = new OptionalDateInputViewModel(dates.ActualDate.DateTime, true);

            var prenotificationDate = dates.PreNotificationDate.HasValue
                ? dates.PreNotificationDate.Value.DateTime : (DateTime?)null;

            PrenotificationDate = new OptionalDateInputViewModel(prenotificationDate, true);
        }
    }
}