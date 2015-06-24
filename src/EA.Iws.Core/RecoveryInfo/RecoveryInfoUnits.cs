namespace EA.Iws.Core.RecoveryInfo
{
    using System.ComponentModel.DataAnnotations;

    public enum RecoveryInfoUnits
    {
        [Display(Name = "per kg")]
        Kilogram = 1,

        [Display(Name = "per tonne")]
        Tonne = 2
    }
}
