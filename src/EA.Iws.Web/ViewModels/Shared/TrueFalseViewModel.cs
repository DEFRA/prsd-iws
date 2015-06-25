namespace EA.Iws.Web.ViewModels.Shared
{
    using System.ComponentModel.DataAnnotations;

    public class TrueFalseViewModel
    {
        [Required(ErrorMessage = "Please answer this question")]
        public bool? Value { get; set; }
    }
}