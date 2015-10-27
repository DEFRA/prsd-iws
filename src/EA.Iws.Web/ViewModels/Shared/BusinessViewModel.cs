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

        [Required]
        [Display(Name = "Organisation name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Organisation type")]
        public string EntityType { get; set; }

        [RequiredIf("EntityType", "Limited company", ErrorMessage = "Companies house number is required")]
        [Display(Name = "Companies House number")]
        public string CompaniesHouseRegistrationNumber { get; set; }

        [RequiredIf("EntityType", "Sole trader", ErrorMessage = "Sole Trader registration number is required")]
        [Display(Name = "Registration number")]
        public string SoleTraderRegistrationNumber { get; set; }

        [RequiredIf("EntityType", "Partnership", ErrorMessage = "Partnership registration number is required")]
        [Display(Name = "Registration number")]
        public string PartnershipRegistrationNumber { get; set; }

        [Display(Name = "Additional registration number")]
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