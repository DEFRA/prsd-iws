namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.Shared
{
    using System.ComponentModel.DataAnnotations;

    public class BusinessViewModel
    {
        [Display(Name = "BusinessName", ResourceType = typeof(BusinessResources))]
        public string Name { get; set; }

        [Display(Name = "RegistrationNumber", ResourceType = typeof(BusinessResources))]
        public string RegistrationNumber { get; set; }

        [Display(Name = "OrgTradingName", ResourceType = typeof(BusinessResources))]
        public string OrgTradingName { get; set; }

        [Display(Name = "AdditionalRegNumber", ResourceType = typeof(BusinessResources))]
        [MaxLength(100, ErrorMessageResourceType = typeof(BusinessResources), ErrorMessageResourceName = "AdditionalRegistrationNumberMaxLength")]
        public string AdditionalRegistrationNumber { get; set; }

        public BusinessViewModel()
        {
        }
        public BusinessViewModel(string businessName)
        {
            Name = businessName;
        }

        public BusinessViewModel(string businessName, string registrationNumber, string additionalRegistrationNumber = "")
        {
            if (!string.IsNullOrEmpty(businessName))
            {
                if (businessName.Contains("T/A"))
                {
                    string[] stringSeparator = new string[] { " T/A " };
                    string[] strBusinessName = businessName.Split(stringSeparator, System.StringSplitOptions.None);
                    Name = strBusinessName[0];
                    OrgTradingName = strBusinessName[1];
                }
                else
                {
                    Name = businessName;
                }
            }
            RegistrationNumber = registrationNumber;
            AdditionalRegistrationNumber = additionalRegistrationNumber;
        }
    }
}