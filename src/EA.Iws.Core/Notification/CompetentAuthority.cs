namespace EA.Iws.Core.Notification
{
    using System.ComponentModel.DataAnnotations;

    public enum CompetentAuthority
    {
        [Display(Name = "Environment Agency (EA)")]
        England = 1,
        [Display(Name = "Scottish Environment Protection Agency (SEPA)")]
        Scotland = 2,
        [Display(Name = "Northern Ireland Environment Agency (NIEA)")]
        NorthernIreland = 3,
        [Display(Name = "Natural Resources Wales (NRW)")]
        Wales = 4
    }
}