namespace EA.Iws.Web.Areas.Admin.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class DateInputViewModel
    {
        [Required]
        [Range(1, 31)]
        public int DecisionDay { get; set; }

        [Required]
        [Range(1, 12)]
        public int DecisionMonth { get; set; }

        [Required]
        [Range(0, 9999999)]
        public int DecisionYear { get; set; }
    }
}