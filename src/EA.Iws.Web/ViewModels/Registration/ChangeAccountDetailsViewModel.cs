namespace EA.Iws.Web.ViewModels.Registration
{
    using System.ComponentModel.DataAnnotations;
    using Views.Registration;

    public class ChangeAccountDetailsViewModel
    {
        [Required(ErrorMessageResourceName = "OptionRequired", ErrorMessageResourceType = typeof(ChangeAccountDetailsResources))]
        public bool? ChangeOptions { get; set; }
    }
}