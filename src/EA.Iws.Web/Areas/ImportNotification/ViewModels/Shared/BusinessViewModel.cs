namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.Shared
{
    using System.ComponentModel.DataAnnotations;

    public class BusinessViewModel
    {
        [Display(Name = "BusinessName", ResourceType = typeof(BusinessResources))]
        public string Name { get; set; }

        [Display(Name = "RegistrationNumber", ResourceType = typeof(BusinessResources))]
        public string RegistrationNumber { get; set; }
        public BusinessViewModel()
        {
        }
        public BusinessViewModel(string businessName)
        {
            Name = businessName;
        }

        public BusinessViewModel(string businessName, string registrationNumber)
        {
            Name = businessName;
            RegistrationNumber = registrationNumber;
        }
    }
}