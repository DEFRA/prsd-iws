namespace EA.Iws.Web.Areas.Admin.ViewModels.TestEmail
{
    using System.ComponentModel.DataAnnotations;

    public class TestEmailViewModel
    {
        [Required(ErrorMessageResourceName = "EmailAddressRequired", ErrorMessageResourceType = typeof(TestEmailViewModelResources))]
        [EmailAddress(ErrorMessageResourceName = "EmailAddressInvalid", ErrorMessageResourceType = typeof(TestEmailViewModelResources), ErrorMessage = null)]
        [Display(Name = "EmailTo", ResourceType = typeof(TestEmailViewModelResources))]
        public string EmailTo { get; set; }
    }
}