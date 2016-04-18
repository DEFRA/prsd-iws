namespace EA.Iws.Web.ViewModels.Registration
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Core.Shared;
    using Prsd.Core.Validation;
    using Views.Registration;

    public class CreateNewOrganisationViewModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrganisationId { get; set; }

        [Required]
        [Display(Name = "OrganisationName", ResourceType = typeof(CreateNewOrganisationResources))]
        public string Name { get; set; }

        [Required]
        [Display(Name = "OrganisationType", ResourceType = typeof(CreateNewOrganisationResources))]
        public BusinessType BusinessType { get; set; }

        [RequiredIf("BusinessType", BusinessType.Other, ErrorMessageResourceName = "DescriptionRequired", ErrorMessageResourceType = typeof(CreateNewOrganisationResources))]
        [Display(Name = "OrganisationType", ResourceType = typeof(CreateNewOrganisationResources))]
        public string OtherDescription { get; set; }
    }
}