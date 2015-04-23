namespace EA.Iws.Web.ViewModels.NotificationApplication
{
    using System.ComponentModel.DataAnnotations;

    public enum UKCompetentAuthorities
    {
        [Display(Name = "Environment Agency")] 
        EnvironmentAgency = 0,
        [Display(Name = "Scottish Environment Protection Agency")] 
        ScottishEnvironmentProtectionAgency = 1,
        [Display(Name = "Northern Ireland Environment Agency")] 
        NorthernIrelandEnvironmentAgency = 2,
        [Display(Name = "Natural Resources Wales")] 
        NaturalResourcesWales = 3
    }
}