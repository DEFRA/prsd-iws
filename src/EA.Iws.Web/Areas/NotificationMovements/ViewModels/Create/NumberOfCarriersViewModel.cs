namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Create
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class NumberOfCarriersViewModel
    {
        [Required]
        [Display(Name = "Number of carriers")]
        [Range(1, 100, ErrorMessage = "Number of carriers must be between 1 and 100")]
        public int? Number { get; set; }

        public IList<int> MovementNumbers { get; set; }

        public MeansOfTransportViewModel MeansOfTransportViewModel { get; set; }

        public NumberOfCarriersViewModel()
        {
        }

        public NumberOfCarriersViewModel(MeansOfTransportViewModel subViewModel, IList<int> movementNumbers)
        {
            MovementNumbers = movementNumbers;
            MeansOfTransportViewModel = subViewModel;
        }
    }
}