namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Create
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class NumberOfCarriersViewModel
    {
        [Required(ErrorMessage = "Please enter the number of carriers")]
        [Display(Name = "Number of carriers")]
        [Range(1, 100, ErrorMessage = "Number of carriers must be between 1 and 100")]
        public int? Number { get; set; }

        public int MovementNumber { get; set; }

        public MeansOfTransportViewModel MeansOfTransportViewModel { get; set; }

        public NumberOfCarriersViewModel()
        {
        }

        public NumberOfCarriersViewModel(MeansOfTransportViewModel subViewModel, int movementNumber)
        {
            MovementNumber = movementNumber;
            MeansOfTransportViewModel = subViewModel;
        }
    }
}