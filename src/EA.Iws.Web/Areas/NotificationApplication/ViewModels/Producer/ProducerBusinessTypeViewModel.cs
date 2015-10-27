namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Producer
{
    using System.ComponentModel.DataAnnotations;
    using Core.Shared;
    using Prsd.Core.Validation;

    public class ProducerBusinessTypeViewModel
    {
        [Required]
        [Display(Name = "Organisation name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Organisation type")]
        public BusinessType? BusinessType { get; set; }

        [RequiredIf("BusinessType", Core.Shared.BusinessType.LimitedCompany, ErrorMessage = "The Registration number field is required")]
        [Display(Name = "Registration number")]
        public virtual string RegistrationNumber { get; set; }

        [Display(Name = "Additional registration number")]
        public string AdditionalRegistrationNumber { get; set; }

        [RequiredIf("BusinessType", Core.Shared.BusinessType.Other, ErrorMessage = "Please enter your organisation type")]
        [Display(Name = "Enter your organisation type")]
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
                BusinessType = BusinessType.Value,
                Name = Name,
                OtherDescription = (BusinessType.Value == Core.Shared.BusinessType.Other) ? OtherDescription : null,
                RegistrationNumber = RegistrationNumber
            };
        }
    }
}