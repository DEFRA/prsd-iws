namespace EA.Iws.Core.Notification
{
    using System.ComponentModel.DataAnnotations;

    public enum UKCompetentAuthority
    {
        [Display(Name = "Environment Agency (EA)", ShortName = "EA", Description = "Environment Agency")]
        England = 1,
        [Display(Name = "Scottish Environment Protection Agency (SEPA)", ShortName = "SEPA", Description = "Scottish Environment Protection Agency")]
        Scotland = 2,
        [Display(Name = "Northern Ireland Environment Agency (NIEA)", ShortName = "NIEA", Description = "Northern Ireland Environment Agency")]
        NorthernIreland = 3,
        [Display(Name = "Natural Resources Wales (NRW)", ShortName = "NRW", Description = "Natural Resources Wales")]
        Wales = 4
    }
}