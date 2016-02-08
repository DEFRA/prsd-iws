namespace EA.Iws.Core.Notification
{
    using System.ComponentModel.DataAnnotations;

    public enum UKCompetentAuthority
    {
        [Display(Name = "Environment Agency (EA)", ShortName = "EA")]
        England = 1,
        [Display(Name = "Scottish Environment Protection Agency (SEPA)", ShortName = "SEPA")]
        Scotland = 2,
        [Display(Name = "Northern Ireland Environment Agency (NIEA)", ShortName = "NIEA")]
        NorthernIreland = 3,
        [Display(Name = "Natural Resources Wales (NRW)", ShortName = "NRW")]
        Wales = 4
    }
}