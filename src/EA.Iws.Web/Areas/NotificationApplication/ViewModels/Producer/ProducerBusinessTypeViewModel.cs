namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Producer
{
    using System.ComponentModel.DataAnnotations;
    using Core.Shared;
    using Prsd.Core.Validation;
    using Views.Producer;
    using Web.ViewModels.Shared;

    public class ProducerBusinessTypeViewModel
    {
        [Required(ErrorMessageResourceName = "OrganisationNameRequired", ErrorMessageResourceType = typeof(AddEditProducerResources))]
        [Display(Name = "OrganisationName", ResourceType = typeof(AddEditProducerResources))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "OrganisationTypeRequired", ErrorMessageResourceType = typeof(AddEditProducerResources))]
        [Display(Name = "OrganisationType", ResourceType = typeof(AddEditProducerResources))]
        public BusinessType? BusinessType { get; set; }

        [RequiredIf("BusinessType", Core.Shared.BusinessType.LimitedCompany, ErrorMessageResourceName = "RegistrationNumberRequired", ErrorMessageResourceType = typeof(AddEditProducerResources))]
        [Display(Name = "RegistrationNumber", ResourceType = typeof(AddEditProducerResources))]
        [MaxLength(100, ErrorMessageResourceType = typeof(BusinessResources), ErrorMessageResourceName = "RegistrationNumberMaxLength")]
        public string RegistrationNumber { get; set; }

        [Display(Name = "AdditionalRegistrationNumber", ResourceType = typeof(AddEditProducerResources))]
        [MaxLength(100, ErrorMessageResourceType = typeof(BusinessResources), ErrorMessageResourceName = "AdditionalRegistrationNumberMaxLength")]
        public string AdditionalRegistrationNumber { get; set; }

        [RequiredIf("BusinessType", Core.Shared.BusinessType.Other, ErrorMessageResourceName = "OtherOrgTypeDescriptionRequired", ErrorMessageResourceType = typeof(AddEditProducerResources))]
        [Display(Name = "OtherOrgType", ResourceType = typeof(AddEditProducerResources))]
        public string OtherDescription { get; set; }

        public bool DisplayAdditionalNumber { get; set; }

        public bool DisplayCompaniesHouseHint { get; set; }

        public ProducerBusinessTypeViewModel()
        {
        }

        public ProducerBusinessTypeViewModel(BusinessInfoData business)
        {
            Name = business.Name;
            BusinessType = business.BusinessType;
            RegistrationNumber = business.RegistrationNumber;
            AdditionalRegistrationNumber = business.AdditionalRegistrationNumber;
            OtherDescription = business.OtherDescription;
        }

        public virtual BusinessInfoData ToBusinessInfoData()
        {
            return new BusinessInfoData
            {
                AdditionalRegistrationNumber = AdditionalRegistrationNumber,
                BusinessType = BusinessType.GetValueOrDefault(),
                Name = Name,
                OtherDescription = (BusinessType.GetValueOrDefault() == Core.Shared.BusinessType.Other) ? OtherDescription : null,
                RegistrationNumber = RegistrationNumber
            };
        }
    }
}