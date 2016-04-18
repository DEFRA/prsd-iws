namespace EA.Iws.Core.Movement
{
    using System.ComponentModel.DataAnnotations;

    public enum QuantityReceivedTolerance
    {
        [Display(Name = "Below")]
        BelowTolerance = 1,
        [Display(Name = "Above")]
        AboveTolerance = 2,
        WithinTolerance = 3
    }
}
