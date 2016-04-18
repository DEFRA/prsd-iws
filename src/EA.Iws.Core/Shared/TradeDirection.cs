namespace EA.Iws.Core.Shared
{
    using System.ComponentModel.DataAnnotations;

    public enum TradeDirection
    {
        [Display(Name = "Export")]
        Export = 1,
        [Display(Name = "Import")]
        Import = 2
    }
}