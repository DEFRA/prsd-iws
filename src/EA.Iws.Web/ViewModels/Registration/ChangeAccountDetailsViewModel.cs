namespace EA.Iws.Web.ViewModels.Registration
{
    using System.ComponentModel.DataAnnotations;

    public class ChangeAccountDetailsViewModel
    {
        [Required(ErrorMessage = "Please answer this question.")]
        public bool? ChangeOptions { get; set; }
    }
}