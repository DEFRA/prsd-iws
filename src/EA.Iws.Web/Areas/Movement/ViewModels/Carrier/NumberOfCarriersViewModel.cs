namespace EA.Iws.Web.Areas.Movement.ViewModels.Carrier
{
    using System.ComponentModel.DataAnnotations;

    public class NumberOfCarriersViewModel
    {
        [Required]
        [Display(Name = "Number of carriers")]
        [Range(1, 100, ErrorMessage = "Number of carriers must be between 1 and 100")]
        public int? Amount { get; set; }

        public MeansOfTransportViewModel MeansOfTransportViewModel { get; set; }
    }
}