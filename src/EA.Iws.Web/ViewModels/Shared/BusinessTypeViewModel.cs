﻿namespace EA.Iws.Web.ViewModels.Shared
{
    using System.ComponentModel.DataAnnotations;
    using Core.Shared;
    using Prsd.Core.Validation;

    public class BusinessTypeViewModel
    {
        [Required(ErrorMessageResourceType = typeof(BusinessResources), ErrorMessageResourceName = "OrgNameRequired")]
        [Display(Name = "OrgName", ResourceType = typeof(BusinessResources))]
        public string Name { get; set; }

        [Display(Name = "OrgTradingName", ResourceType = typeof(BusinessResources))]
        public string OrgTradingName { get; set; }

        [Display(Name = "BusinessTypeHeader", ResourceType = typeof(BusinessResources))]
        public string BusinessTypeHeader { get; set; }

        public bool? IsUkBased { get; set; }

        [Required(ErrorMessageResourceType = typeof(BusinessResources), ErrorMessageResourceName = "OrgTypeRequired")]
        [Display(Name = "OrgType", ResourceType = typeof(BusinessResources))]
        public BusinessType? BusinessType { get; set; }

        [Required(ErrorMessageResourceType = typeof(BusinessResources), ErrorMessageResourceName = "RegNumberRequired")]
        [Display(Name = "RegistrationNumber", ResourceType = typeof(BusinessResources))]
        [MaxLength(100, ErrorMessageResourceType = typeof(BusinessResources), ErrorMessageResourceName = "RegistrationNumberMaxLength")]
        public virtual string RegistrationNumber { get; set; }

        [Display(Name = "AdditionalRegNumber", ResourceType = typeof(BusinessResources))]
        [MaxLength(100, ErrorMessageResourceType = typeof(BusinessResources), ErrorMessageResourceName = "AdditionalRegistrationNumberMaxLength")]
        public string AdditionalRegistrationNumber { get; set; }

        [RequiredIf("BusinessType", Core.Shared.BusinessType.Other, ErrorMessageResourceType = typeof(BusinessResources), ErrorMessageResourceName = "OtherOrgTypeRequired")]
        [Display(Name = "OtherOrgType", ResourceType = typeof(BusinessResources))]
        public string OtherDescription { get; set; }

        public bool DisplayAdditionalNumber { get; set; }

        public bool DisplayCompaniesHouseHint { get; set; }

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

        public virtual BusinessInfoData ToBusinessInfoData()
        {
            return new BusinessInfoData
            {
                AdditionalRegistrationNumber = AdditionalRegistrationNumber,
                BusinessType = BusinessType.GetValueOrDefault(),
                Name = Name,
                OtherDescription = OtherDescription,
                RegistrationNumber = RegistrationNumber
            };
        }
    }
}