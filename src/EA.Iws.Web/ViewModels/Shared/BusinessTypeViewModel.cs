namespace EA.Iws.Web.ViewModels.Shared
{
    using System.ComponentModel.DataAnnotations;
    using Core.Shared;
    using Prsd.Core.Validation;

    public class BusinessTypeViewModel
    {
        [Required]
        [Display(Name = "Organisation name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Organisation type")]
        public BusinessType? BusinessType { get; set; }

        [Required]
        [Display(Name = "Registration number")]
        public string RegistrationNumber { get; set; }

        [Display(Name = "Additional registration number")]
        public string AdditionalRegistrationNumber { get; set; }

        [RequiredIf("BusinessType", Core.Shared.BusinessType.Other, "Please enter your organisation type")]
        [Display(Name = "Enter your organisation type")]
        public string OtherDescription { get; set; }

        public BusinessTypeViewModel()
        {
        }

        public BusinessTypeViewModel(BusinessInfoData business)
        {
            Name = business.Name;
            BusinessType = business.BusinessType;
            RegistrationNumber = business.RegistrationNumber;
            AdditionalRegistrationNumber = business.AdditionalRegistrationNumber;
            OtherDescription = business.OtherDescription;
        }

        public BusinessInfoData ToBusinessInfoData()
        {
            return new BusinessInfoData
            {
                AdditionalRegistrationNumber = AdditionalRegistrationNumber,
                BusinessType = BusinessType.Value,
                Name = Name,
                OtherDescription = OtherDescription,
                RegistrationNumber = RegistrationNumber
            };
        }
    }
}