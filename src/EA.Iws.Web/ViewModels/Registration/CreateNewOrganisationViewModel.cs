namespace EA.Iws.Web.ViewModels.Registration
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Core.Shared;
    using Prsd.Core.Validation;

    public class CreateNewOrganisationViewModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrganisationId { get; set; }

        [Required]
        [Display(Name = "Organisation name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Organisation type")]
        public BusinessType BusinessType { get; set; }

        [RequiredIf("BusinessType", BusinessType.Other, "Description is required")]
        [Display(Name = "Organisation type")]
        public string OtherDescription { get; set; }
    }
}