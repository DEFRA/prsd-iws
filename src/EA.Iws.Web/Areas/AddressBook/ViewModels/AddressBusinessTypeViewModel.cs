namespace EA.Iws.Web.Areas.AddressBook.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using AddressBook.Views.Edit;
    using Core.Shared;
    using Prsd.Core.Validation;
    using Web.ViewModels.Shared;

    public class AddressBusinessTypeViewModel
    {
        [Required(ErrorMessageResourceName = "OrganisationNameRequired", ErrorMessageResourceType = typeof(AddEditResource))]
        [Display(Name = "OrganisationName", ResourceType = typeof(AddEditResource))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "OrganisationTypeRequired", ErrorMessageResourceType = typeof(AddEditResource))]
        [Display(Name = "OrganisationType", ResourceType = typeof(AddEditResource))]
        public BusinessType? BusinessType { get; set; }

        [RequiredIf("BusinessType", Core.Shared.BusinessType.LimitedCompany, ErrorMessageResourceName = "RegistrationNumberRequired", ErrorMessageResourceType = typeof(AddEditResource))]
        [Display(Name = "RegistrationNumber", ResourceType = typeof(AddEditResource))]
        [MaxLength(100, ErrorMessageResourceType = typeof(BusinessResources), ErrorMessageResourceName = "RegistrationNumberMaxLength")]
        public string RegistrationNumber { get; set; }

        [RequiredIf("BusinessType", Core.Shared.BusinessType.Other, ErrorMessageResourceName = "OtherOrgTypeDescriptionRequired", ErrorMessageResourceType = typeof(AddEditResource))]
        [Display(Name = "OtherOrgType", ResourceType = typeof(AddEditResource))]
        public string OtherDescription { get; set; }

        public AddressBusinessTypeViewModel()
        {
        }

        public AddressBusinessTypeViewModel(BusinessInfoData business)
        {
            Name = business.Name;
            BusinessType = business.BusinessType;
            RegistrationNumber = business.RegistrationNumber;
            OtherDescription = business.OtherDescription;
        }

        public virtual BusinessInfoData ToBusinessInfoData()
        {
            return new BusinessInfoData
            {
                BusinessType = BusinessType.GetValueOrDefault(),
                Name = Name,
                OtherDescription = (BusinessType.GetValueOrDefault() == Core.Shared.BusinessType.Other) ? OtherDescription : null,
                RegistrationNumber = RegistrationNumber
            };
        }
    }
}