namespace EA.Iws.Web.Areas.Movement.ViewModels.NumberOfPackages
{
    using System.ComponentModel.DataAnnotations;

    public class NumberOfPackagesViewModel
    {
        [Required(ErrorMessage = "Please enter the number of packages")]
        [Range(1, 10000, ErrorMessage = "Number of packages must be between 1 and 10,000")]
        [Display(Name = "Enter the number")]
        public int? Number { get; set; }
    }
}