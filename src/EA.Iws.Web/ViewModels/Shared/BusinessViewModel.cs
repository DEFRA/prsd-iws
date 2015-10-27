namespace EA.Iws.Web.ViewModels.Shared
{
    using System.ComponentModel.DataAnnotations;
    using Core.Shared;
    using Prsd.Core.Helpers;
    using Prsd.Core.Validation;

    public class BusinessViewModel
    {
        private const string SoleTrader = "Sole trader";
        private const string Partnership = "Partnership";
        private const string LimitedCompany = "Limited company";

        public BusinessViewModel()
        {
        }

        public BusinessViewModel(BusinessInfoData business)
        {
            Name = business.Name;
            EntityType = EnumHelper.GetDisplayName(business.BusinessType);
            AdditionalRegistrationNumber = business.AdditionalRegistrationNumber;

            switch (EntityType)
            {
                case (SoleTrader):
                    SoleTraderRegistrationNumber = business.RegistrationNumber;
                    break;
                case (Partnership):
                    PartnershipRegistrationNumber = business.RegistrationNumber;
                    break;
                default:
                    CompaniesHouseRegistrationNumber = business.RegistrationNumber;
                    break;
            }
        }

        [Required(ErrorMessageResourceType = typeof(BusinessResources), ErrorMessageResourceName = "OrgNameRequired")]
        [Display(Name = "OrgName", ResourceType = typeof(BusinessResources))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(BusinessResources), ErrorMessageResourceName = "OrgTypeRequired")]
        [Display(Name = "OrgType", ResourceType = typeof(BusinessResources))]
        public string EntityType { get; set; }

        [RequiredIf("EntityType", "Limited company", ErrorMessageResourceType = typeof(BusinessResources), ErrorMessageResourceName = "CompanyHouseRequired")]
        [Display(Name = "CompaniesHouseNumber", ResourceType = typeof(BusinessResources))]
        public string CompaniesHouseRegistrationNumber { get; set; }

        [RequiredIf("EntityType", "Sole trader", ErrorMessageResourceType = typeof(BusinessResources), ErrorMessageResourceName = "SoleTraderRegNoRequired")]
        [Display(Name = "RegistrationNumber", ResourceType = typeof(BusinessResources))]
        public string SoleTraderRegistrationNumber { get; set; }

        [RequiredIf("EntityType", "Partnership", ErrorMessageResourceType = typeof(BusinessResources), ErrorMessageResourceName = "PartnershipRegNoRequired")]
        [Display(Name = "RegistrationNumber", ResourceType = typeof(BusinessResources))]
        public string PartnershipRegistrationNumber { get; set; }

        [Display(Name = "AdditionalRegNumber", ResourceType = typeof(BusinessResources))]
        public string AdditionalRegistrationNumber { get; set; }

        public static explicit operator BusinessData(BusinessViewModel businessViewModel)
        {
            var business = new BusinessData
            {
                Name = businessViewModel.Name,
                EntityType = businessViewModel.EntityType,
                AdditionalRegistrationNumber = businessViewModel.AdditionalRegistrationNumber
            };

            switch (businessViewModel.EntityType)
            {
                case (SoleTrader):
                    business.RegistrationNumber = businessViewModel.SoleTraderRegistrationNumber;
                    break;
                case (Partnership):
                    business.RegistrationNumber = businessViewModel.PartnershipRegistrationNumber;
                    break;
                default:
                    business.RegistrationNumber = businessViewModel.CompaniesHouseRegistrationNumber;
                    break;
            }

            return business;
        }

        public static explicit operator BusinessViewModel(BusinessData business)
        {
            var businessViewModel = new BusinessViewModel
            {
                Name = business.Name,
                EntityType = business.EntityType,
                AdditionalRegistrationNumber = business.AdditionalRegistrationNumber
            };

            switch (businessViewModel.EntityType)
            {
                case (SoleTrader):
                    businessViewModel.SoleTraderRegistrationNumber = business.RegistrationNumber;
                    break;
                case (Partnership):
                    businessViewModel.PartnershipRegistrationNumber = business.RegistrationNumber;
                    break;
                default:
                    businessViewModel.CompaniesHouseRegistrationNumber = business.RegistrationNumber;
                    break;
            }

            return businessViewModel;
        }
    }
}