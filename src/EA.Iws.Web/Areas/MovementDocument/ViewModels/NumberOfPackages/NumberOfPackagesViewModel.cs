namespace EA.Iws.Web.Areas.MovementDocument.ViewModels.NumberOfPackages
{
    using System.ComponentModel.DataAnnotations;

    public class NumberOfPackagesViewModel
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Number of packages must be greater than 0")]
        [Display(Name = "Enter the number")]
        public int? Number { get; set; }
    }
}