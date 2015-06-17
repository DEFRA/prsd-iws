namespace EA.Iws.Web.ViewModels.Shared
{
    using System.ComponentModel.DataAnnotations;

    public class TrueFalseViewModel
    {
        [Required]
        public bool? Value { get; set; }
    }
}